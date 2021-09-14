using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    [CreateAssetMenu(menuName = "World/Terrain/Cave Data")]
    public class CaveGenerationData : ScriptableObject
    {
        public float Frequency = 1f;
        public float Offset = 345345f;
        [Range(0f, 1f)]
        public float CellularJitter = 0.3f;
        public float ThresholdSurface = 1f;
        public float ThresholdFloor = 0.2f;
        public float SurfaceHeight = 0f;
        public float FloorHeight = -128f;
        public FastNoiseLite.CellularDistanceFunction distanceFunction;
        public FastNoiseLite.CellularReturnType returnType;
    }
}