using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Scripts.Gameplay.WorldScripts
{
    public struct ChunkUpdateRequestMessage : NetworkMessage
    {
        public Vector3Int ChunkPos;
    }
}