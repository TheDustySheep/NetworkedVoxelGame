using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Networking
{
    public struct CompressedChunkDataSegment
    {
        public int SegmentIndex;
        public int[] HashData;
        public ushort[] CountData;
    }
}