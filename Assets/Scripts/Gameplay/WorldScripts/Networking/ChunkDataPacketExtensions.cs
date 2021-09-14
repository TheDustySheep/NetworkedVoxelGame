using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Networking
{
    public static class ChunkDataPacketExtensions
    {
        public static void WriteToData(this ChunkDataPacket packet, ChunkData data)
        {
            int startIndex = packet.Segment * StaticWorldData.CHUNK_SIZE_3_DIV_8;

            for (int i = 0; i < packet.HashKeys.Length; i++)
            {

            }
        }

        public static ChunkDataPacket[] ReadToPackets(this ChunkData data)
        {
            ChunkDataPacket[] packets = new ChunkDataPacket[8];

            List<IntData> segments = new List<IntData>();

            for (int i = 0; i < 8; i++)
            {
                segments.Clear();
                ChunkDataPacket packet = new ChunkDataPacket()
                {
                    Segment = (byte)i
                };

                int StartIndex = i * StaticWorldData.CHUNK_SIZE_3_DIV_8;

                IntData index = new IntData() { Data = data[StartIndex].HashKey };
                ushort count = 0;

                for (int j = 1; j < StaticWorldData.CHUNK_SIZE_3_DIV_8; j++)
                {
                    if (data[StartIndex + j].HashKey != index.Data)
                    {
                        index.Count = count;
                        segments.Add(index);
                        index = new IntData() { Data = data[StartIndex + j].HashKey };
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }
                }

                packet.HashKeys = segments.ToArray();

                packets[i] = packet;
            }

            return packets;
        }
    }
}