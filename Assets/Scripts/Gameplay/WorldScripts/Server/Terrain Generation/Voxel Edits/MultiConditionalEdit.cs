using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Multi Conditional Edit")]
    public class MultiConditionalEdit : VoxelEditBase
    {
        [SerializeField] Edit[] Edits = new Edit[0];

        public override VoxelData EditVoxel(VoxelData current)
        {
            for (int i = 0; i < Edits.Length; i++)
            {
                if (Edits[i].Condition.name.GetStableHashCode() == current.HashKey)
                {
                    return new VoxelData(Edits[i].Output.name.GetStableHashCode());
                }
            }

            return current;
        }

        [System.Serializable]
        private struct Edit
        {
            public VoxelType Condition;
            public VoxelType Output;
        }
    }
}