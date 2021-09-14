using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public class TerrainGenerator
    {
        TerrainGenerationSettings settings;

        public TerrainGenerator(TerrainGenerationSettings _settings)
        {
            settings = _settings;
        }

        public void GenerateTerrain(TerrainGenerationData terrainData, ChunkData data, Vector3Int globalOffset)
        {
            for (int x = 0; x < StaticWorldData.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < StaticWorldData.CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < StaticWorldData.CHUNK_SIZE; z++)
                    {
                        Vector3Int local = new Vector3Int(x, y, z);
                        Vector3Int global = globalOffset + local;

                        data[x, y, z] = GetData(terrainData, global, local);
                    }
                }
            }
        }

        private VoxelData GetData(TerrainGenerationData terrainData, Vector3Int global, Vector3Int local)
        {
            string key;

            int terrainHeight = terrainData.terrainHeight[local.x, local.z];
            int dirtDepth = terrainData.dirtDepth[local.x, local.z];
            int stone2Height = terrainData.stone2Height[local.x, local.z];
            int stone3Height = terrainData.stone3Height[local.x, local.z];
            int stone4Height = terrainData.stone4Height[local.x, local.z];


            if (global.y > terrainHeight)
            {
                if (global.y < settings.waterHeight + 1)
                    key = "Water";
                else
                    key = "Air";
            }
            else if (global.y == terrainHeight)
                key = "Grass";
            else if (global.y > terrainHeight - dirtDepth)
                key = "Dirt";
            else
            {
                if (global.y > stone2Height)
                    key = "Stone_1";
                else if (global.y > stone3Height)
                    key = "Stone_2";
                else if (global.y > stone4Height)
                    key = "Stone_3";
                else
                    key = "Stone_4";
            }

            return new VoxelData(key);
        }
    }
}