using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts
{
    public static class StaticWorldData
    {
        public static readonly int CHUNK_SIZE_POWER = 5; // 2^5 = 32
        public static readonly int CHUNK_SIZE = (int)Mathf.Pow(2, CHUNK_SIZE_POWER);
        public static readonly int CHUNK_SIZE_2 = CHUNK_SIZE * CHUNK_SIZE;
        public static readonly int CHUNK_SIZE_3 = CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE;
        public static readonly int CHUNK_SIZE_3_DIV_8 = CHUNK_SIZE_3 / 8;
        public static readonly int CHUNK_SIZE_3_6 = CHUNK_SIZE_3 * 6;
        public static readonly int THREAD_GROUPS_XZ = Mathf.Max(1, CHUNK_SIZE / 16);
        public static readonly int THREAD_GROUPS_Y = Mathf.Max(1, CHUNK_SIZE / 4);

        private static readonly int VOXEL_BITSHIFT_AMOUNT = 32 - CHUNK_SIZE_POWER; // int size - 5 data points

        public static readonly Vector3Int[] OFFSETS = new Vector3Int[6]
        {
            new Vector3Int( 0, 1, 0),
            new Vector3Int( 0,-1, 0),
            new Vector3Int( 0, 0, 1),
            new Vector3Int( 0, 0,-1),
            new Vector3Int( 1, 0, 0),
            new Vector3Int(-1, 0, 0),
        };

        public static Vector3Int ConvectWorldVoxelToChunk(Vector3Int _pos)
        {
            if (_pos.x < 0)
                _pos.x -= CHUNK_SIZE - 1;
            if (_pos.y < 0)
                _pos.y -= CHUNK_SIZE - 1;
            if (_pos.z < 0)
                _pos.z -= CHUNK_SIZE - 1;
            
            Vector3Int pos = _pos / CHUNK_SIZE;
            
            return pos;
            
            //_pos = _pos.BitShiftRight(CHUNK_SIZE_POWER);
            //return _pos;
        }

        public static Vector3Int ConvectWorldVoxelToLocalVoxel(Vector3Int _pos)
        {
            Vector3Int pos = _pos.UnsignedMod(CHUNK_SIZE);
            return pos;

            //bool xSign = _pos.x > 0;
            //bool ySign = _pos.y > 0;
            //bool zSign = _pos.z > 0;
            //
            //_pos = _pos.BitShiftLeft(VOXEL_BITSHIFT_AMOUNT);
            //_pos = _pos.BitShiftRight(VOXEL_BITSHIFT_AMOUNT);
            //
            //_pos.x = xSign ? _pos.x : CHUNK_SIZE - _pos.x;
            //_pos.y = ySign ? _pos.y : CHUNK_SIZE - _pos.y;
            //_pos.z = zSign ? _pos.z : CHUNK_SIZE - _pos.z;
            //
            //return _pos;
        }
    }
}