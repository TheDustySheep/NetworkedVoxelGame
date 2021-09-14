using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Scripts.Gameplay.WorldScripts
{
    [System.Serializable]
    public struct ChunkUpdateMessage : NetworkMessage
    {
        public Vector3Int ChunkPos;
        public ChunkData Data;

        public ChunkUpdateMessage(ChunkPos pos, ChunkData chunkData)
        {
            ChunkPos = pos;
            Data = chunkData;
        }
    }
}