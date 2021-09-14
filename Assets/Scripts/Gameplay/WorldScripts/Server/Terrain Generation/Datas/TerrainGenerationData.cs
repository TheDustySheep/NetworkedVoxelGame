using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public class TerrainGenerationData
    {
        public int[,] terrainHeight = new int[StaticWorldData.CHUNK_SIZE, StaticWorldData.CHUNK_SIZE];
        public int[,] dirtDepth = new int[StaticWorldData.CHUNK_SIZE, StaticWorldData.CHUNK_SIZE];
        public int[,] stone2Height = new int[StaticWorldData.CHUNK_SIZE, StaticWorldData.CHUNK_SIZE];
        public int[,] stone3Height = new int[StaticWorldData.CHUNK_SIZE, StaticWorldData.CHUNK_SIZE];
        public int[,] stone4Height = new int[StaticWorldData.CHUNK_SIZE, StaticWorldData.CHUNK_SIZE];
    }
}