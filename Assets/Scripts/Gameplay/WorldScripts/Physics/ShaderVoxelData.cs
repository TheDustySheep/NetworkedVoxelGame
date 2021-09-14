using Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.PhysicsScripts
{
    public struct ShaderVoxelData
    {
        public int Collidable;

        public ShaderVoxelData(Indexer<VoxelType> indexer, VoxelData voxelData)
        {
            Collidable = indexer.GetItem(voxelData.HashKey).UseCollider ? 1 : 0;
        }
    }
}