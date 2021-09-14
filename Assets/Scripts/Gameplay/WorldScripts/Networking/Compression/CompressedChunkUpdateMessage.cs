using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Networking
{
    public struct CompressedChunkUpdateMessage : NetworkMessage
    {
        public Vector3Int ChunkPos;
        public CompressedChunkDataSegment data;

        public CompressedChunkUpdateMessage(ChunkPos pos, ChunkData chunkData)
        {
            ChunkPos = pos;
            data = ChunkDataCompressor.Compress16(chunkData);
        }
    }
}