using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Ore")]
    public class OreGeneration : ScriptableObject
    {
        [Header("Spawn Settings")]
        public float spawnCutoff = 0.95f;
        public float frequency = 400f;
        public float offset = 0f;

        [Header("Spawn Range")]
        public int MinHeight = -128;
        public int MaxHeight = 0;

        [Header("Voxel Edit")]
        public VoxelEditBase oreEdit;
    }
}