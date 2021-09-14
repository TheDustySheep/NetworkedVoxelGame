using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts.Client.MeshGeneration;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class Chunk
    {
        public ChunkData Data;
        public bool isBeingGenerated = false;
        public bool needsMeshRegeneration = false;
        public ChunkPos chunkPos;
        public MeshObjectDrawMesh meshObject;

        public Chunk(ChunkPos _chunkPos, ChunkData _data, MeshObjectDrawMesh _meshObject)
        {
            chunkPos = _chunkPos;
            Data = _data;
            meshObject = _meshObject;

            needsMeshRegeneration = true;
        }

        public void NeighbourUpdate()
        {
            needsMeshRegeneration = true;
        }

        public void UpdateChunk(ChunkData chunkData)
        {
            Data = chunkData;
            needsMeshRegeneration = true;

            Utils.Signals.Get<OnChunkUpdate>().Dispatch(chunkPos, Data);
        }

        public VoxelData GetVoxel(Vector3Int localPos)
        {
            return Data[localPos];
        }

        public void EditVoxel(Vector3Int localPos, VoxelData data)
        {
            Data[localPos] = data;
            needsMeshRegeneration = true;

            Utils.Signals.Get<OnChunkUpdate>().Dispatch(chunkPos, Data);
        }
    }
}