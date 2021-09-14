using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain Generation Settings")]
    public class TerrainGenerationSettings : ScriptableObject
    {
        [Header("Seed")]
        public int Seed = 1337;

        [Header("Caves")]
        public bool enableCaves = true;
        public CaveGenerationData caveGeneration;

        [Header("Ores")]
        public OreGeneration[] ores;

        [Header("Trees")]
        public float treeThreshold = 0.8f;
        public TreeGeneration treeGeneration;
        
        [Header("Layers")]
        public TerrainLayer GrassLayer;
        public TerrainLayer DirtLayer;
        public TerrainLayer Stone2Layer;
        public TerrainLayer Stone3Layer;
        public TerrainLayer Stone4Layer;

        [Header("Other")]
        public int waterHeight = 4;
    }
}