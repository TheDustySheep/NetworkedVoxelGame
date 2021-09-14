using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class RunLengthEncoder
    {
        public static void Encode<T>(T[] input, out ushort[] counts, out T[] values)
        {
            int totalCounts = 1;
            T _currentVal = default;

            for (int i = 0; i < input.Length; i++)
            {
                if (!_currentVal.Equals(input[i]))
                {
                    _currentVal = input[i];
                    totalCounts++;
                }
            }

            counts = new ushort[totalCounts];
            values = new T[totalCounts];

            //List<ushort> _counts = new List<ushort>();
            //List<T> _values = new List<T>();

            T currentValue = input[0];
            ushort currentCount = 1;
            int currentIndex = 0;

            for (int i = 1; i < input.Length; i++)
            {
                if (currentValue.Equals(input[i]))
                {
                    currentCount++;
                }
                else
                {
                    //_values.Add(currentValue);
                    //_counts.Add(currentCount);
                    values[currentIndex] = currentValue;
                    counts[currentIndex] = currentCount;

                    currentIndex++;
                    currentValue = input[i];
                    currentCount = 1;
                }
            }

            values[totalCounts - 1] = currentValue;
            counts[totalCounts - 1] = currentCount;

            //_values.Add(currentValue);
            //_counts.Add(currentCount);
            //
            //values = _values.ToArray();
            //counts = _counts.ToArray();
        }

        public static void Decode<T>(out T[] output, ushort[] counts, T[] values)
        {
            List<T> _output = new List<T>();

            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];

                for (int j = 0; j < counts[i]; j++)
                {
                    _output.Add(value);
                }
            }

            output = _output.ToArray();
        }

        public static void Encode(int[] input, out ushort[] counts, out int[] values)
        {
            int totalCounts = 1;
            int _currentVal = default;

            for (int i = 0; i < input.Length; i++)
            {
                if (_currentVal != input[i])
                {
                    _currentVal = input[i];
                    totalCounts++;
                }
            }

            counts = new ushort[totalCounts];
            values = new int[totalCounts];

            //List<ushort> _counts = new List<ushort>();
            //List<T> _values = new List<T>();

            int currentValue = input[0];
            ushort currentCount = 1;
            int currentIndex = 0;

            for (int i = 1; i < input.Length; i++)
            {
                if (currentValue == input[i])
                {
                    currentCount++;
                }
                else
                {
                    //_values.Add(currentValue);
                    //_counts.Add(currentCount);
                    values[currentIndex] = currentValue;
                    counts[currentIndex] = currentCount;

                    currentIndex++;
                    currentValue = input[i];
                    currentCount = 1;
                }
            }

            values[totalCounts - 1] = currentValue;
            counts[totalCounts - 1] = currentCount;

            //_values.Add(currentValue);
            //_counts.Add(currentCount);
            //
            //values = _values.ToArray();
            //counts = _counts.ToArray();
        }

        public static void Decode(out int[] output, ushort[] counts, int[] values)
        {
            List<int> _output = new List<int>();

            for (int i = 0; i < values.Length; i++)
            {
                int value = values[i];

                for (int j = 0; j < counts[i]; j++)
                {
                    _output.Add(value);
                }
            }

            output = _output.ToArray();
        }
    }
}