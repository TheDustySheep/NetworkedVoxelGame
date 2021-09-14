using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    [System.Serializable]
    public class ChunkData
    {
        public VoxelData[] data = new VoxelData[StaticWorldData.CHUNK_SIZE_3];

        public VoxelData this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }

        public VoxelData this [int x, int y, int z]
        {
            get => data[(StaticWorldData.CHUNK_SIZE_2 * z) + (StaticWorldData.CHUNK_SIZE * y) + x];
            set => data[(StaticWorldData.CHUNK_SIZE_2 * z) + (StaticWorldData.CHUNK_SIZE * y) + x] = value;
        }

        public VoxelData this[Vector3Int pos]
        {
            get => data[(StaticWorldData.CHUNK_SIZE_2 * pos.z) + (StaticWorldData.CHUNK_SIZE * pos.y) + pos.x];
            set => data[(StaticWorldData.CHUNK_SIZE_2 * pos.z) + (StaticWorldData.CHUNK_SIZE * pos.y) + pos.x] = value;
        }
    }
}