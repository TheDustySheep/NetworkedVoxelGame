using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts
{
    public class OnChunkUpdate : ASignal<ChunkPos, ChunkData> { }
    public class OnChunkDestroy : ASignal<ChunkPos> { }
}
