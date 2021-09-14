using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Utils
{
    public class Grid2D<T>
    {
        public Action OnValueChange;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public T[,] values;

        public Grid2D(int width, int height)
        {
            Width = width;
            Height = height;

            values = new T[width, height];
        }

        public virtual T GetValue(Vector2Int pos)
        {
            return values[pos.x, pos.y];
        }

        public virtual void SetValue(T value, Vector2Int pos)
        {
            values[pos.x, pos.y] = value;
            OnValueChange?.Invoke();
        }

        public virtual void SetValueRange(T value, Vector2Int pos, int range)
        {
            for (int x = -range; x < range + 1; x++)
            {
                if (x + pos.x < 0)
                    continue;

                if (x + pos.x >= Width)
                    break;

                for (int y = -range; y < range + 1; y++)
                {
                    if (y + pos.y < 0)
                        continue;

                    if (y + pos.y >= Height)
                        break;

                    values[x + pos.x, y + pos.y] = value; 
                }
            }

            OnValueChange?.Invoke();
        }

        public virtual Grid2D<T> ShallowClone()
        {
            Grid2D<T> newGrid = new Grid2D<T>(Width, Height);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    newGrid.values[x, y] = values[x, y];
                }
            }

            return newGrid;
        }

        public bool IsOutOfRange(Vector2Int pos) => values.IsOutOfRange(pos);
    }
}