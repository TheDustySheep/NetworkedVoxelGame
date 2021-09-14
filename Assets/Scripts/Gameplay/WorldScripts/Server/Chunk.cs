using Scripts.Gameplay.WorldScripts.Server.TerrainGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server
{
    public class Chunk
    {
        public ChunkPos ChunkPos;
        public ChunkData Data;

        public Chunk(ChunkPos _chunkPos, WorldGenerator terrainGenerator, ref Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            ChunkPos = _chunkPos;
            Data = new ChunkData();

            terrainGenerator.Generate(ChunkPos, Data, ref edits);
        }

        public VoxelData GetVoxel(Vector3Int localPos) => Data[localPos];
        public void EditVoxel(Vector3Int localPos, VoxelData data) => Data[localPos] = data;
    }
}