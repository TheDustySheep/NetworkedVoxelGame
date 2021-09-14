using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Scripts.Gameplay.WorldScripts
{
    public struct VoxelUpdateRequestMessage : NetworkMessage
    {
        public Vector3Int pos;
        public VoxelData data;
    }
}