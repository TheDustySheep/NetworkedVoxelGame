using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class Vector3IntExtensions
    {
        public static Vector3Int Mod(this Vector3Int pos, int div)
        {
            return new Vector3Int(pos.x % div, pos.y % div, pos.z % div);
        }

        public static Vector3Int UnsignedMod(this Vector3Int pos, int div)
        {
            return new Vector3Int(mod(pos.x, div), mod(pos.y, div), mod(pos.z, div));
        }

        private static int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static Vector3Int BitShiftLeft(this Vector3Int a, int b)
        {
            a.x = Mathf.Abs(a.x) << b;
            a.y = Mathf.Abs(a.y) << b;
            a.z = Mathf.Abs(a.z) << b;
            return a;
        }

        public static Vector3Int BitShiftRight(this Vector3Int a, int b)
        {
            a.x = Mathf.Abs(a.x) >> b;
            a.y = Mathf.Abs(a.y) >> b;
            a.z = Mathf.Abs(a.z) >> b;
            return a;
        }
    }
}