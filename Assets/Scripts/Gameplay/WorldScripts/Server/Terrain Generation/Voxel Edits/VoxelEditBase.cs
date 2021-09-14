using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public abstract class VoxelEditBase : ScriptableObject
    {
        public abstract VoxelData EditVoxel(VoxelData current);
    }
}