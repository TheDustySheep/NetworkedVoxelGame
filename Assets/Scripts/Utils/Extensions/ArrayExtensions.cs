using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class ArrayExtensions
    {
        public static bool IsOutOfRange<T>(this T[] array, int index)
        {
            return index < 0 || index >= array.Length;
        }

        public static bool IsOutOfRange<T>(this T[,] array, Vector2Int pos)
        {
            return pos.x < 0 || pos.x >= array.GetLength(0) ||
                   pos.y < 0 || pos.y >= array.GetLength(1);
        }

        public static bool IsOutOfRange<T>(this T[,,] array, Vector3Int pos)
        {
            return pos.x < 0 || pos.x >= array.GetLength(0) ||
                   pos.y < 0 || pos.y >= array.GetLength(1) ||
                   pos.z < 0 || pos.z >= array.GetLength(2);
        }
    }
}