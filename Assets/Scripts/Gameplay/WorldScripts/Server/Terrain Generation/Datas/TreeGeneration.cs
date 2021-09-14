using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Tree Generation")]
    public class TreeGeneration : ScriptableObject
    {
        [Header("Voxel Edits")]
        public VoxelEditBase LeavesEdit;
        public VoxelEditBase LogEdit;
    }
}