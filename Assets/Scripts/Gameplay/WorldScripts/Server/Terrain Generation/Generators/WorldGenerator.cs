using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public class WorldGenerator
    {
        private static int AIR_HASH = "Air".GetStableHashCode();

        FastNoiseLite noise;
        TerrainGenerationSettings settings;

        TerrainGenerator terrainGenerator;
        CaveGenerator caveGenerator;

        public WorldGenerator(TerrainGenerationSettings _settings) 
        {
            settings = _settings;
            noise = new FastNoiseLite(settings.Seed);
            terrainGenerator = new TerrainGenerator(settings);
            caveGenerator = new CaveGenerator(settings);
        }

        public void Generate(Vector3Int globalChunkPos, ChunkData data, ref Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            Vector3Int globalOffset = globalChunkPos * StaticWorldData.CHUNK_SIZE;

            TerrainGenerationData terrainData = GenerateTerrainData(globalOffset);

            terrainGenerator.GenerateTerrain(terrainData, data, globalOffset);

            if (settings.enableCaves)
                caveGenerator.GenerateCaves(terrainData, data, globalOffset);

            GenerateTrees(terrainData, data, globalOffset, edits);

            foreach (var oreType in settings.ores)
            {
                GenerateOre(oreType, terrainData, data, globalOffset, edits);
            }
        }

        private void GenerateTrees(TerrainGenerationData terrainData, ChunkData data, Vector3Int globalOffset, Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            for (int x = 0; x < StaticWorldData.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < StaticWorldData.CHUNK_SIZE; z++)
                {
                    for (int y = 0; y < StaticWorldData.CHUNK_SIZE; y++)
                    {
                        if (y + globalOffset.y == terrainData.terrainHeight[x, z] + 1)
                        {
                            if (data[new Vector3Int(x, y, z)].HashKey == AIR_HASH)
                            {
                                if (noise.GetSimplexNoise01(new Vector3(x, y, z) + globalOffset, 325f, 234324f) > settings.treeThreshold)
                                {
                                    StructureGenerator.GenerateTree(settings.treeGeneration, new Vector3Int(x, y, z) + globalOffset, edits);
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void GenerateOre(OreGeneration oreType, TerrainGenerationData terrainData, ChunkData data, Vector3Int globalOffset, Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            if (globalOffset.y > oreType.MaxHeight || globalOffset.y + StaticWorldData.CHUNK_SIZE < oreType.MaxHeight)
                return;

            for (int x = 0; x < StaticWorldData.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < StaticWorldData.CHUNK_SIZE; z++)
                {
                    for (int y = 0; y < StaticWorldData.CHUNK_SIZE; y++)
                    {
                        int yPos = y + globalOffset.y;
                        if (yPos > oreType.MinHeight - 1 && yPos < oreType.MaxHeight + 1)
                        {
                            if (noise.GetSimplexNoise01(new Vector3(x, y, z) + globalOffset, oreType.frequency, oreType.offset) > oreType.spawnCutoff)
                            {
                                StructureGenerator.GenerateOre(oreType, new Vector3Int(x, y, z) + globalOffset, edits);
                            }

                            break;
                        }
                    }
                }
            }
        }



        private TerrainGenerationData GenerateTerrainData(Vector3Int globalOffset)
        {
            TerrainGenerationData data = new TerrainGenerationData();

            for (int x = 0; x < StaticWorldData.CHUNK_SIZE; x++)
            {
                for (int z = 0; z < StaticWorldData.CHUNK_SIZE; z++)
                {
                    Vector3Int pos = new Vector3Int(x, 0, z) + globalOffset;

                    data.terrainHeight[x, z] = GetHeight(pos, settings.GrassLayer);
                    data.dirtDepth[x, z] = GetHeight(pos, settings.DirtLayer);

                    data.stone2Height[x, z] = GetHeight(pos, settings.Stone2Layer);
                    data.stone3Height[x, z] = GetHeight(pos, settings.Stone3Layer);
                    data.stone4Height[x, z] = GetHeight(pos, settings.Stone4Layer);
                }
            }

            return data;
        }

        private int GetHeight(Vector3Int pos, TerrainLayer layer)
        {
            Vector2 XYPos = new Vector2(pos.x, pos.z);
            float height = Mathf.Lerp(layer.Range.x, layer.Range.y, noise.GetPerlinNoise01(XYPos, layer.Frequency, layer.Offset));
            return (int)height;
        }
    }
}