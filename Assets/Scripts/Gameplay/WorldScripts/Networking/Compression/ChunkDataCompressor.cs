using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Networking
{
    public static class ChunkDataCompressor
    {
        public static CompressedChunkDataSegment Compress16(ChunkData data)
        {
            int[] input = new int[data.data.Length];

            for (int i = 0; i < data.data.Length; i++)
            {
                input[i] = data.data[i].HashKey;
            }

            RunLengthEncoder.Encode(input, out var counts, out var values);
            //int inputSize = input.Length * 4;
            //int outputSize = (counts.Length * 2) + (values.Length * 4);
            //float percent = (1f - (((float)outputSize) / ((float)inputSize))) * 100f;
            //
            //Debug.Log($"Reduced size from: {inputSize} bytes, to: {outputSize} bytes, {percent}%");

            return new CompressedChunkDataSegment()
            {
                CountData = counts,
                HashData = values,
            };
        }
        
        public static void Decompress16(ChunkData data, CompressedChunkDataSegment segment)
        {
            RunLengthEncoder.Decode(out var output, segment.CountData, segment.HashData);

            for (int i = 0; i < output.Length; i++)
            {
                data[i] = new VoxelData(output[i]);
            }
        }
    }
}