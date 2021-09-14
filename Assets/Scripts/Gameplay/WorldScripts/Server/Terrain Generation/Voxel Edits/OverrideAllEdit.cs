using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Override Edit")]
    public class OverrideAllEdit : VoxelEditBase
    {
        [SerializeField] VoxelType type;

        int TypeHash;

        private void Awake()
        {
            TypeHash = type.name.GetStableHashCode();
        }

        public override VoxelData EditVoxel(VoxelData current)
        {
            return new VoxelData(TypeHash);
        }
    }
}