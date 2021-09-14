using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts.Client.MeshGeneration;
using Mirror;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class ClientDimension : MonoBehaviour
    {
        [SerializeField] Utils.ScriptableReference.Vector3Reference playerPosition;
        [SerializeField] ClientLegacy.ClientDimensionSettings dimensionSettings;
        [SerializeField] ChunkRenderingSettings chunkRenderingSettings;

        Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos, Chunk>();
        ChunkMeshGeneratorHandler generatorHandler;
        ChunkSpawningHandler spawningHandler;
        ActiveChunkHandler activeChunkHandler;
        VoxelIndexer indexer = new VoxelIndexer();

        //Active Chunks
        public int ActiveChunkCount => chunks.Count;
        public int requiredChunkCount => activeChunkHandler == null ? 0 : activeChunkHandler.activeChunks.Count;
        public int NeedRegenCount => generatorHandler == null ? 0 : generatorHandler.NeedRegenCount;

        //Queues
        public int VoxelDataQueueCount => generatorHandler == null ? 0 : generatorHandler.VoxelDataQueueCount;
        public int GPUQueueCount => generatorHandler == null ? 0 : generatorHandler.GPUQueueCount;
        public int MeshDataQueueCount => generatorHandler == null ? 0 : generatorHandler.MeshDataCalculationQueue;
        public int ApplyMeshCount => generatorHandler == null ? 0 : generatorHandler.MeshDataCalculationQueue;
        

        private void Awake()
        {
            indexer.UpdateIndex("World/Voxel Types");
        }

        private void Start()
        {
            activeChunkHandler = new ActiveChunkHandler(dimensionSettings, playerPosition);
            generatorHandler = new ChunkMeshGeneratorHandler(chunks, chunkRenderingSettings, indexer, dimensionSettings);
            spawningHandler = new ChunkSpawningHandler(chunks, transform, chunkRenderingSettings, activeChunkHandler);

            StartCoroutine(generatorHandler.CheckForChunksNeedingUpdate());

            for (int i = 0; i < dimensionSettings.VoxelDataConcurrent; i++)
            {
                StartCoroutine(generatorHandler.GenerateChunkData());
            }

            for (int i = 0; i < dimensionSettings.GPUDataSimultanious; i++)
            {
                StartCoroutine(generatorHandler.CalculateFaces());
            }
            
            StartCoroutine(generatorHandler.CalculateMeshData());
            StartCoroutine(generatorHandler.ApplyMeshData());
            //StartCoroutine(generatorHandler.UpdateColliders());

            StartCoroutine(spawningHandler.UpdateActiveChunks());
        }

        private void Update()
        {
            foreach (var chunk in chunks)
            {
                chunk.Value.meshObject.Draw();
            }
        }

        public void RequestEdit(Vector3Int voxelPos, VoxelData data)
        {
            EditVoxelClient(voxelPos, data);

            VoxelUpdateRequestMessage request = new VoxelUpdateRequestMessage
            {
                pos = voxelPos,
                data = data
            };

            NetworkClient.Send(request);
        }

        private void EditVoxelClient(Vector3Int voxelPos, VoxelData data)
        {
            Vector3Int chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(voxelPos);
            Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(voxelPos);

            if (chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.EditVoxel(localPos, data);
            }
        }

        public VoxelType GetVoxelType(Vector3Int globalPos)
        {
            Vector3Int chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(globalPos);
            Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(globalPos);

            if (chunks.TryGetValue(chunkPos, out var chunk))
                return indexer.GetItem(chunk.GetVoxel(localPos).HashKey);
            else
                return default;
        }

        public VoxelData GetVoxelData(Vector3Int globalPos)
        {
            Vector3Int chunkPos = StaticWorldData.ConvectWorldVoxelToChunk(globalPos);
            Vector3Int localPos = StaticWorldData.ConvectWorldVoxelToLocalVoxel(globalPos);

            if (chunks.TryGetValue(chunkPos, out var chunk))
                return chunk.GetVoxel(localPos);
            else
                return default;
        }
    }
}