using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class Chunk
    {
        IChunkRenderer chunkRenderer;
        IChunkMeshGenerator meshGenerator;
        
        public ChunkData Data;
        public bool needsMeshRegeneration = false;
        public bool isRegenerating = false;

        Mesh tMesh;
        Mesh sMesh;

        public Chunk(IChunkRenderer _chunkRenderer, IChunkMeshGenerator _meshGenerator, ChunkData _chunkData, Mesh _tMesh, Mesh _sMesh)
        {
            tMesh = _tMesh;
            sMesh = _sMesh;
            chunkRenderer = _chunkRenderer;
            meshGenerator = _meshGenerator;
            Data = _chunkData;
            needsMeshRegeneration = true;
        }

        public void UpdateChunk(ChunkData _chunkData)
        {
            Data = _chunkData;
            needsMeshRegeneration = true;
        }

        public IEnumerator Regenerate(ChunkData[] _neighbours)
        {
            if (isRegenerating)
                yield break;

            isRegenerating = true;
            needsMeshRegeneration = false;
            yield return meshGenerator.Regenerate(Data, _neighbours, sMesh, false);
            yield return meshGenerator.Regenerate(Data, _neighbours, tMesh, true);
            yield return chunkRenderer.OnRegenerate();
            isRegenerating = false;
        }

        public void Draw() => chunkRenderer.Draw();

        public void Destroy()
        {
            chunkRenderer.Destroy();
        }

        public void EditVoxel(Vector3Int localPos, VoxelData data)
        {
            Data[localPos] = data;
            needsMeshRegeneration = true;
        }
    }
}