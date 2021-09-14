using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Networking
{
    public struct ChunkDataPacket
    {
        public byte Segment;

        public IntData[] HashKeys;
        public IntData[] State;
    }

    public struct IntData
    {
        public ushort Count;
        public int Data;
    }
}