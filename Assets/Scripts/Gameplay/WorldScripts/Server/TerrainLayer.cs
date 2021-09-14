using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server
{
    [System.Serializable]
    public class TerrainLayer
    {
        public float Frequency = 1f;
        public float Offset = 0f;
        public int Octaves = 1;
        public Vector2Int Range = new Vector2Int(0, 1);
    }
}