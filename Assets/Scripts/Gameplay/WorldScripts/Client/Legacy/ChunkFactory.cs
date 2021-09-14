using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class ChunkFactory
    {
        readonly ChunkRenderingSettings chunkSettings;
        readonly Indexer<VoxelType> indexer;
        readonly Transform parent;

        public ChunkFactory(ChunkRenderingSettings _chunkSettings, Indexer<VoxelType> _indexer, Transform _parent)
        {
            chunkSettings = _chunkSettings;
            indexer = _indexer;
            parent = _parent;
        }

        public Chunk GenerateChunk(ChunkPos chunkPos, ChunkData chunkData)
        {
            Mesh smesh = new Mesh();
            Mesh tmesh = new Mesh();

            IChunkMeshGenerator meshGenerator = new ChunkMeshGenerator(chunkSettings, indexer);
            IChunkRenderer chunkRenderer = new ChunkRendererGameObject(chunkPos, chunkSettings, smesh, tmesh, parent);

            Chunk chunk = new Chunk
            (
                chunkRenderer,
                meshGenerator,
                chunkData,
                tmesh,
                smesh
            );

            return chunk;
        }
    }
}