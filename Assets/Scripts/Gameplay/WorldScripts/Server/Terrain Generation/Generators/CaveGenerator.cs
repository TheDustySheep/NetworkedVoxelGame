using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public class CaveGenerator
    {
        FastNoiseLite noise1;
        FastNoiseLite noise2;

        private TerrainGenerationSettings settings;
        private CaveGenerationData caveData;

        int AIR_HASHCODE = "Air".GetStableHashCode();

        public CaveGenerator(TerrainGenerationSettings _settings)
        {
            settings = _settings;
            caveData = settings.caveGeneration;
            noise1 = new FastNoiseLite(settings.Seed);
            noise2 = new FastNoiseLite(settings.Seed + 643634);
        }

        public void GenerateCaves(TerrainGenerationData terrainData, ChunkData data, Vector3Int globalOffset)
        {
            noise1.SetCellularDistanceFunction(caveData.distanceFunction);
            noise1.SetCellularReturnType(caveData.returnType);
            noise2.SetCellularDistanceFunction(caveData.distanceFunction);
            noise2.SetCellularReturnType(caveData.returnType);

            noise1.SetCellularJitter(caveData.CellularJitter);
            noise2.SetCellularJitter(caveData.CellularJitter);

            for (int y = 0; y < StaticWorldData.CHUNK_SIZE; y++)
            {
                float HeightPercent = Mathf.Clamp01(Mathf.InverseLerp(caveData.FloorHeight, caveData.SurfaceHeight, globalOffset.y + y));
                float Threshold = Mathf.Lerp(caveData.ThresholdFloor, caveData.ThresholdSurface, HeightPercent);

                for (int x = 0; x < StaticWorldData.CHUNK_SIZE; x++)
                {
                    for (int z = 0; z < StaticWorldData.CHUNK_SIZE; z++)
                    {
                        Vector3Int local = new Vector3Int(x, y, z);
                        Vector3Int global = globalOffset + local;

                        float noise1Val = noise1.GetNoise0To1(global, caveData.Frequency, caveData.Offset, FastNoiseLite.NoiseType.Cellular);
                        float noise2Val = noise2.GetNoise0To1(global, caveData.Frequency, caveData.Offset + 235465f, FastNoiseLite.NoiseType.Cellular);

                        float value = Mathf.Min(noise1Val, noise2Val);

                        if (value > Threshold)
                        {
                            data[local] = new VoxelData(AIR_HASHCODE);
                        }
                    }
                }
            }
        }
    }
}