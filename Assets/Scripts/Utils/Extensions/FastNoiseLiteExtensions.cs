using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class FastNoiseLiteExtensions
    {
        private static readonly FastNoiseLite.NoiseType PerlinType = FastNoiseLite.NoiseType.Perlin;
        private static readonly FastNoiseLite.NoiseType SimplexType = FastNoiseLite.NoiseType.OpenSimplex2S;

        public static float GetNoiseNeg1To1(this FastNoiseLite noise, Vector3 pos, float frequency, float offset, FastNoiseLite.NoiseType noiseType)
        {
            noise.SetNoiseType(noiseType);
            pos += Vector3.one * offset;
            pos *= frequency;
            return noise.GetNoise(pos.x, pos.y, pos.z);
        }

        public static float GetNoise0To1(this FastNoiseLite noise, Vector3 pos, float frequency, float offset, FastNoiseLite.NoiseType noiseType)
        {
            noise.SetNoiseType(noiseType);
            pos += Vector3.one * offset;
            pos *= frequency;
            return (noise.GetNoise(pos.x, pos.y, pos.z) + 1f) * 0.5f;
        }

        public static float GetPerlinNoiseNeg1To1(this FastNoiseLite noise, Vector2 pos, float frequency, float offset)
        {
            noise.SetNoiseType(PerlinType);
            return noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency);
        }

        public static float GetPerlinNoiseNeg1To1(this FastNoiseLite noise, Vector3 pos, float frequency, float offset)
        {
            noise.SetNoiseType(PerlinType);
            return noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency, (pos.z + offset) * frequency);
        }

        public static float GetSimplexNoiseNeg1To1(this FastNoiseLite noise, Vector2 pos, float frequency, float offset)
        {
            noise.SetNoiseType(SimplexType);
            return noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency);
        }

        public static float GetSimplexNoiseNeg1To1(this FastNoiseLite noise, Vector3 pos, float frequency, float offset)
        {
            noise.SetNoiseType(SimplexType);
            return noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency, (pos.z + offset) * frequency);
        }

        public static float GetPerlinNoise01(this FastNoiseLite noise, Vector2 pos, float frequency, float offset)
        {
            noise.SetNoiseType(PerlinType);
            return (noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency) + 1f) * 0.5f;
        }

        public static float GetPerlinNoise01(this FastNoiseLite noise, Vector3 pos, float frequency, float offset)
        {
            noise.SetNoiseType(PerlinType);
            return (noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency, (pos.z + offset) * frequency) + 1f) * 0.5f;
        }

        public static float GetSimplexNoise01(this FastNoiseLite noise, Vector2 pos, float frequency, float offset)
        {
            noise.SetNoiseType(SimplexType);
            return (noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency) + 1f) * 0.5f;
        }

        public static float GetSimplexNoise01(this FastNoiseLite noise, Vector3 pos, float frequency, float offset)
        {
            noise.SetNoiseType(SimplexType);
            return (noise.GetNoise((pos.x + offset) * frequency, (pos.y + offset) * frequency, (pos.z + offset) * frequency) + 1f) * 0.5f;
        }

        /// <summary>
        /// Remaps 0 -> 1 noise to range
        /// </summary>
        /// <param name="range">Exclusive range</param>
        /// <param name="value">Value between 0 -> 1</param>
        /// <returns></returns>
        public static int RemapNoise01ToRange(this FastNoiseLite noise, Vector2Int range, float value)
        {
            int spread = range.y - range.x;
            value *= spread;
            return Mathf.Clamp(Mathf.FloorToInt(value), 0, spread - 1) + range.x;
        }

        public static int RandomRange(this FastNoiseLite noise, Vector2Int range, Vector2 pos, float frequency, float offset)
        {
            return noise.RemapNoise01ToRange(range, noise.GetSimplexNoise01(pos, frequency, offset));
        }
    }
}