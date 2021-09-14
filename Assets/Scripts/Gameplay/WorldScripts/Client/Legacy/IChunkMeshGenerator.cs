using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    public interface IChunkMeshGenerator
    {
        public IEnumerator Regenerate(ChunkData data, ChunkData[] neighbours, Mesh mesh, bool isTransparent);
    }
}