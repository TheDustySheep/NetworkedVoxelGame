using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public struct ShaderVoxelTypeData
    {
        public int config;
        public int generateCriteria;

        //Y
        public Vector2Int yPosIndex;
        public Vector2Int yNegIndex;
        //Z
        public Vector2Int zPosIndex;
        public Vector2Int zNegIndex;
        //X
        public Vector2Int xPosIndex;
        public Vector2Int xNegIndex;

        public static implicit operator ShaderVoxelTypeData(VoxelType type)
        {
            return new ShaderVoxelTypeData()
            {
                config =
                    (type.DrawSelf ? 1 : 0) |
                    (type.DrawNeighboursSimilar ? 2 : 0) |
                    (type.DrawNeighboursDifferent ? 4 : 0) |
                    (type.AllSidesSame ? 8 : 0) |
                    (type.IsFluid ? 16 : 0),


                generateCriteria =
                        (type.Transparent ? 1 : 0),

                xPosIndex = type.xPosIndex,
                xNegIndex = type.xNegIndex,
                yPosIndex = type.yPosIndex,
                yNegIndex = type.yNegIndex,
                zPosIndex = type.zPosIndex,
                zNegIndex = type.zNegIndex
            };
        }

        public static ShaderVoxelTypeData Default = new ShaderVoxelTypeData()
        {
            config = 2
        };
    }
}