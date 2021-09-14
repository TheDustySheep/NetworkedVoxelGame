using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public class VoxelIndexer : Indexer<VoxelType>
    {
        public Dictionary<int, int> hashConversion = new Dictionary<int, int>();
        public ShaderVoxelTypeData[] shaderVoxelTypes = new ShaderVoxelTypeData[0];

        public override void UpdateIndex(string filePath)
        {
            base.UpdateIndex(filePath);

            hashConversion.Clear();
            hashConversion.Add(0, 0);
            shaderVoxelTypes = new ShaderVoxelTypeData[index.Count + 1];

            shaderVoxelTypes[0] = ShaderVoxelTypeData.Default;

            int i = 1;
            foreach (var item in index)
            {
                hashConversion.Add(item.Key, i);
                shaderVoxelTypes[i] = item.Value;
                i++;
            }
        }
    }
}