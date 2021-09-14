using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public class ChunkMeshGeneratorHandler
    {
        VoxelIndexer indexer;
        ChunkMeshGenerator meshGenerator;
        Dictionary<ChunkPos, Chunk> chunks;
        ClientLegacy.ClientDimensionSettings dimensionSettings;

        public int VoxelDataQueueCount => VoxelDataQueue.Count;
        public int GPUQueueCount => GPUQueue.Count;
        public int MeshDataCalculationQueue => MeshDataQueue.Count;
        public int ApplyMeshCount => ApplyMeshQueue.Count;

        public int NeedRegenCount = 0;

        Queue<ChunkPos> VoxelDataQueue = new Queue<ChunkPos>();
        Queue<(ChunkPos, ShaderVoxelData[], ShaderVoxelData[])> GPUQueue = new Queue<(ChunkPos, ShaderVoxelData[], ShaderVoxelData[])>();
        ConcurrentQueue<(ChunkPos, ChunkMeshGenerator.FaceData)> MeshDataQueue = new ConcurrentQueue<(ChunkPos, ChunkMeshGenerator.FaceData)>();
        ConcurrentQueue<(ChunkPos, ChunkMeshGenerator.MeshData)> ApplyMeshQueue = new ConcurrentQueue<(ChunkPos, ChunkMeshGenerator.MeshData)>();

        public ChunkMeshGeneratorHandler(Dictionary<ChunkPos, Chunk> _chunks, ChunkRenderingSettings settings, VoxelIndexer _indexer, ClientLegacy.ClientDimensionSettings _dimensionSettings)
        {
            indexer = _indexer;
            chunks = _chunks;
            dimensionSettings = _dimensionSettings;
            meshGenerator = new ChunkMeshGenerator(settings, indexer);
        }

        public void QueueChunkForUpdate(Chunk chunk)
        {
            if (!chunk.isBeingGenerated)
            {
                chunk.isBeingGenerated = true;
                chunk.needsMeshRegeneration = false;

                VoxelDataQueue.Enqueue(chunk.chunkPos);
            }
        }

        public IEnumerator CheckForChunksNeedingUpdate()
        {
            while (true)
            {
                int count = 0;
                foreach (var chunk in chunks.Values)
                {
                    if (chunk.needsMeshRegeneration)
                    {
                        QueueChunkForUpdate(chunk);
                        count++;
                    }
                }
                NeedRegenCount = count;
                yield return null;
            }
        }

        public IEnumerator GenerateChunkData()
        {
            List<ChunkPos> _chunks = new List<ChunkPos>();
            while (true)
            {
                if (VoxelDataQueue.Count > 0)
                {
                    _chunks.Clear();

                    for (int i = 0; i < dimensionSettings.VoxelDataPerCycle && VoxelDataQueue.Count > 0; i++)
                    {
                        _chunks.Add(VoxelDataQueue.Dequeue());
                    }

                    ChunkData[] chunkData = new ChunkData[_chunks.Count];
                    ChunkData[][] neighbourData = new ChunkData[_chunks.Count][];

                    for (int i = 0; i < _chunks.Count; i++)
                    {
                        chunkData[i] = chunks[_chunks[i]].Data;
                        neighbourData[i] = GetNeighbours(_chunks[i]);
                    }

                    (ChunkPos, ShaderVoxelData[], ShaderVoxelData[])[] datas = new (ChunkPos, ShaderVoxelData[], ShaderVoxelData[])[_chunks.Count];

                    yield return new WaitForThreadedTask(() =>
                    {
                        for (int i = 0; i < _chunks.Count; i++)
                        {
                            datas[i].Item1 = _chunks[i];
                            datas[i].Item2 = ConvertVoxelData(chunkData[i]);
                            datas[i].Item3 = ConvertNeighbours(neighbourData[i]);
                        }
                    });

                    for (int i = 0; i < datas.Length; i++)
                    {
                        GPUQueue.Enqueue(datas[i]);
                    }

                    _chunks.Clear();
                }

                yield return null;
            }
        }


        //Convert the voxel data into faces
        public IEnumerator CalculateFaces()
        {
            while (true)
            {
                if (GPUQueue.Count > 0)
                {
                    var item = GPUQueue.Dequeue();
                    ChunkMeshGenerator.FaceData faceData = new ChunkMeshGenerator.FaceData();
                    yield return meshGenerator.Regenerate(item.Item2, item.Item3, faceData);
                    MeshDataQueue.Enqueue((item.Item1, faceData));
                }

                yield return null;
            }
        }

        //Convert the face data into mesh data
        public IEnumerator CalculateMeshData()
        {
            while (true)
            {
                if (MeshDataQueue.Count > 0)
                {
                    yield return new WaitForThreadedTask(() =>
                    {
                        while (MeshDataQueue.Count > 0)
                        {
                            if (MeshDataQueue.TryDequeue(out var item))
                            {
                                var meshData = meshGenerator.GenerateMeshData(item.Item2);
                                ApplyMeshQueue.Enqueue((item.Item1, meshData));
                            }
                        }
                    });
                }

                yield return null;
            }
        }

        //Apply the mesh data
        public IEnumerator ApplyMeshData()
        {
            while (true)
            {
                while (ApplyMeshQueue.Count > 0)
                {
                    if (ApplyMeshQueue.TryDequeue(out var item))
                    {
                        if (chunks.TryGetValue(item.Item1, out var chunk))
                        {
                            meshGenerator.ApplyMeshData(chunk.meshObject.mesh, item.Item2);
                            chunk.isBeingGenerated = false;
                        }
                    }
                }

                yield return null;
            }
        }

        /*
        public IEnumerator UpdateColliders()
        {
            while (true)
            {
                if (chunkColliderQueue.Count > 0)
                {
                    ChunkPos[] positions = chunkColliderQueue.ToArray();
                    chunkColliderQueue.Clear();

                    List<int> meshIDS = new List<int>();

                    for (int i = 0; i < positions.Length; i++)
                    {
                        if (chunks.TryGetValue(positions[i], out var chunk))
                        {
                            meshIDS.AddRange(chunk.meshIDs);
                        }
                    }

                    if (meshIDS.Count > 0)
                    {
                        yield return new WaitForThreadedTask(() =>
                        {
                            for (int i = 0; i < meshIDS.Count; i++)
                            {
                                Physics.BakeMesh(meshIDS[i], false);
                            }
                        });
                    }

                    for (int i = 0; i < positions.Length; i++)
                    {
                        if (chunks.TryGetValue(positions[i], out var chunk))
                        {
                            chunk.UpdateCollider();
                            chunk.isBeingGenerated = false;
                        }
                    }
                }

                yield return null;
            }
        }
        */

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

        private ShaderVoxelData[] ConvertVoxelData(ChunkData data)
        {
            ShaderVoxelData[] cubeData = new ShaderVoxelData[StaticWorldData.CHUNK_SIZE_3];

            for (int i = 0; i < StaticWorldData.CHUNK_SIZE_3; i++)
            {
                cubeData[i] = new ShaderVoxelData(indexer, data[i]);
            }

            return cubeData;
        }

        private ShaderVoxelData[] ConvertNeighbours(ChunkData[] datas)
        {
            ShaderVoxelData[] cubeData = new ShaderVoxelData[StaticWorldData.CHUNK_SIZE_3_6];
            ChunkData data;

            for (int i = 0; i < 6; i++)
            {
                int offset = i * StaticWorldData.CHUNK_SIZE_3;

                data = datas[i];

                if (data != null)
                {
                    for (int j = 0; j < StaticWorldData.CHUNK_SIZE_3; j++)
                    {
                        cubeData[offset + j] = new ShaderVoxelData(indexer, data[j]);
                    }
                }
            }

            return cubeData;
        }
    }
}