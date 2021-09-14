using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    public struct ChunkPos
    {
        public int x;
        public int y;
        public int z;

        public ChunkPos(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static implicit operator Vector3Int(ChunkPos chunkPos) => new Vector3Int(chunkPos.x, chunkPos.y, chunkPos.z);
        public static implicit operator ChunkPos(Vector3Int pos) => new ChunkPos(pos.x, pos.y, pos.z);
    }
}