using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    [CreateAssetMenu(menuName = "World/Voxel Type")]
    public class VoxelType : ScriptableObject
    {
        [Header("Voxel Settings")]
        public bool DrawSelf = true;
        public bool DrawNeighboursSimilar = false;
        public bool DrawNeighboursDifferent = false;
        public bool IsFluid = false;
        public bool UseCollider = true;

        [Header("Mesh Criteria")]
        public bool Transparent = false;

        [Header("Texturing Settings")]
        public bool AllSidesSame = true;

        public Vector2Int xPosIndex;
        public Vector2Int xNegIndex;
        //Y
        public Vector2Int yPosIndex;
        public Vector2Int yNegIndex;
        //Z
        public Vector2Int zPosIndex;
        public Vector2Int zNegIndex;
    }
}