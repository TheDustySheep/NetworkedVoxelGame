using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public struct ShaderVoxelData
    {
        public int id;
        public int state;

        public ShaderVoxelData(VoxelIndexer indexer, VoxelData voxelData)
        {
            id = indexer.hashConversion[voxelData.HashKey];
            state = 0;
        }
    }
}