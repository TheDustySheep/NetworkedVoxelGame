using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Mirror;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class Dimension : MonoBehaviour
    {
        [SerializeField] public Transform playerTransform;
        NetworkConnection connectionToServer;
        [SerializeField] Transform dimensionTransform;
        [SerializeField] ChunkRenderingSettings chunkSettings;
        [SerializeField] ClientDimensionSettings dimensionSettings;
        Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos, Chunk>();
        Indexer<VoxelType> indexer = new Indexer<VoxelType>();
        ChunkFactory chunkFactory;

        int activeMeshUpdates = 0;

        HashSet<ChunkPos> requiredChunks = new HashSet<ChunkPos>();

        private void Awake()
        {
            indexer.UpdateIndex("World/Voxel Types");
            chunkFactory = new ChunkFactory(chunkSettings, indexer, dimensionTransform);

            StartCoroutine(ChunkMeshRegenerator());
            StartCoroutine(SendChunkUpdates());
            StartCoroutine(UpdateActiveChunks());

            NetworkClient.RegisterHandler<ChunkUpdateMessage>(UpdateChunk);
        }

        public void Initilize(NetworkConnection _connectionToServer)
        {
            connectionToServer = _connectionToServer;
        }

        private void Update()
        {
            Draw();
        }

        private IEnumerator UpdateActiveChunks()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / dimensionSettings.recalculateChunksInterval);

                if (playerTransform == null)
                    continue;

                Vector3 playerPos = playerTransform.position;
                ChunkPos playerChunkPos = new Vector3Int((int)playerPos.x, 0, (int)playerPos.z) / StaticWorldData.CHUNK_SIZE;
                
                yield return new WaitForThreadedTask(() =>
                {
                    requiredChunks = RequiredChunkManager.GetChunksCylinder(playerChunkPos, dimensionSettings.renderDistance, dimensionSettings.worldHeights);
                });

                foreach (var pos in requiredChunks)
                {
                    if (!chunks.ContainsKey(pos))
                    {
                        RequestChunk(pos);
                    }
                }

                List<ChunkPos> chunksToRemove = new List<ChunkPos>();
                foreach (var pos in chunks)
                {
                    if (!requiredChunks.Contains(pos.Key))
                    {
                        chunksToRemove.Add(pos.Key);
                    }
                }

                foreach (var pos in chunksToRemove)
                {
                    RemoveChunk(pos);
                }
            }
        }

        public void UpdateChunk(ChunkUpdateMessage message)
        {
            if (!requiredChunks.Contains(message.ChunkPos))
                return;

            ChunkData chunkData = message.Data;

            if (chunks.TryGetValue(message.ChunkPos, out var chunk))
            {
                chunk.UpdateChunk(chunkData);
            }
            else
            {
                SpawnChunk(chunkData, message.ChunkPos);
            }

            for (int i = 0; i < 6; i++)
            {
                if (chunks.TryGetValue(message.ChunkPos + StaticWorldData.OFFSETS[i], out var nchunk))
                {
                    nchunk.needsMeshRegeneration = true;
                }
            }
        }

        private void SpawnChunk(ChunkData chunkData, ChunkPos chunkPos)
        {
            chunks.Add(chunkPos, chunkFactory.GenerateChunk(chunkPos, chunkData));
        }

        private void RemoveChunk(ChunkPos chunkPos)
        {
            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.Destroy();
                chunks.Remove(chunkPos);
            }
            
        }

        private IEnumerator ChunkMeshRegenerator()
        {
            while (true)
            {
                int count = dimensionSettings.MeshesGeneratedPerTick - activeMeshUpdates;

                if (count > 0)
                {
                    Chunk[] _chunks = new Chunk[count];
                    ChunkPos[] _chunkPoses = new ChunkPos[count];

                    int i = 0;
                    foreach (var _chunk in chunks)
                    {
                        if (i == count)
                            break;

                        if (_chunk.Value.needsMeshRegeneration && !_chunk.Value.isRegenerating)
                        {
                            _chunks[i] = _chunk.Value;
                            _chunkPoses[i] = _chunk.Key;
                            i++;
                        }
                    }

                    for (int j = 0; j < i; j++)
                    {
                        StartCoroutine(UpdateChunkMesh(_chunks[j], _chunkPoses[j]));
                    }
                }

                yield return null;
            }
        }

        private IEnumerator UpdateChunkMesh(Chunk chunk, ChunkPos chunkPos)
        {
            activeMeshUpdates++;
            yield return chunk.Regenerate(GetNeighbours(chunkPos));
            activeMeshUpdates--;
        }

        private ChunkData[] GetNeighbours(Vector3Int chunkPos)
        {
            ChunkData[] neighbours = new ChunkData[6];

            for (int i = 0; i < 6; i++)
            {
                if (chunks.TryGetValue(chunkPos + StaticWorldData.OFFSETS[i], out var chunk))
                {
                    neighbours[i] = chunk.Data;
                }
            }

            return neighbours;
        }

        public void Draw()
        {
            foreach (var chunk in chunks.Values)
            {
                chunk.Draw();
            }
        }

        public void RequestEdit(Vector3Int voxelPos, VoxelData data)
        {
            OnVoxelUpdateRequest(voxelPos, data);

            VoxelUpdateRequestMessage request = new VoxelUpdateRequestMessage
            {
                pos = voxelPos,
                data = data
            };

            NetworkClient.Send(request);
        }

        public void OnVoxelUpdateRequest(Vector3Int voxelPos, VoxelData data)
        {
            Vector3Int chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(voxelPos);
            Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(voxelPos);

            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.EditVoxel(localPos, data);
            }
        }

        private Vector3Int ConvectToChunk(Vector3Int _pos)
        {
            if (_pos.x < 0)
                _pos.x -= StaticWorldData.CHUNK_SIZE - 1;
            if (_pos.y < 0)
                _pos.y -= StaticWorldData.CHUNK_SIZE - 1;
            if (_pos.z < 0)
                _pos.z -= StaticWorldData.CHUNK_SIZE - 1;

            Vector3Int pos = _pos / StaticWorldData.CHUNK_SIZE;

            return pos;
        }

        Queue<ChunkPos> chunkRequestQueue = new Queue<ChunkPos>();
        HashSet<ChunkPos> chunkRequestHashSet = new HashSet<ChunkPos>();

        public void RequestChunk(ChunkPos chunkPos)
        {
            if (!chunkRequestHashSet.Contains(chunkPos))
            {
                chunkRequestQueue.Enqueue(chunkPos);
                chunkRequestHashSet.Add(chunkPos);
            }
        }

        private IEnumerator SendChunkUpdates()
        {
            while (true)
            {
                for (int i = 0; i < dimensionSettings.chunkRequestsPerTick; i++)
                {
                    if (chunkRequestQueue.Count == 0)
                        break;

                    ChunkUpdateRequestMessage request = new ChunkUpdateRequestMessage
                    {
                        ChunkPos = chunkRequestQueue.Dequeue()
                    };

                    chunkRequestHashSet.Remove(request.ChunkPos);

                    connectionToServer.Send(request);
                }

                yield return null;
            }
        }
    }
}