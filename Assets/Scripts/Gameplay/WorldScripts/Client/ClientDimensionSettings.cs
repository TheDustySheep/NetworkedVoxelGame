using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    [CreateAssetMenu(menuName = "World/Client Dimension Settings")]
    public class ClientDimensionSettings : ScriptableObject
    {
        [Header("Spawning")]
        public int chunkSpawnsPerTick = 4;
        public float recalculateChunksInterval = 0.5f;

        [Header("Voxel Data")]
        public int VoxelDataConcurrent = 4;
        public int VoxelDataPerCycle = 128;

        [Header("GPU Data")]
        public int GPUDataSimultanious = 32;

        [Header("Networking")]
        public int chunkRequestsPerTick = 4;

        [Header("Render Distance")]
        public int renderDistance = 5;
        public Vector2Int worldHeights = new Vector2Int(-4, 3);
        public ComputeShader activeChunkShader;

        [Header("Legacy")]
        public int MeshesGeneratedPerTick = 4;
        public bool useGraphicsDrawMeshRendering = false;
    }
}