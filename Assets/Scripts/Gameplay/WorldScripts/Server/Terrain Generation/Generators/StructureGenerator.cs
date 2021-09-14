using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server.TerrainGeneration
{
    public static class StructureGenerator
    {
        public static void GenerateTree(TreeGeneration treeGen, Vector3Int basePosition, Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            for (int i = 0; i < 5; i++)
            {
                Add(edits, treeGen.LogEdit, new Vector3Int(0, i, 0) + basePosition);
            }

            for (int x = -2; x < 3; x++)
            {
                for (int y = -2; y < 3; y++)
                {
                    for (int z = -2; z < 3; z++)
                    {
                        Add(edits, treeGen.LeavesEdit, new Vector3Int(x, y + 5, z) + basePosition);
                    }
                }
            }
        }

        public static void GenerateOre(OreGeneration oreType, Vector3Int basePosition, Dictionary<Vector3Int, VoxelEditBase> edits)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        Add(edits, oreType.oreEdit, basePosition + new Vector3Int(x, y, z));
                    }
                }
            }
        }

        private static void Add(Dictionary<Vector3Int, VoxelEditBase> edits, VoxelEditBase data, Vector3Int pos, bool overwrite = false)
        {
            if (edits.ContainsKey(pos))
            {
                if (overwrite)
                    edits.Remove(pos);
                else
                    return;
            }

            edits.Add(pos, data);
        }
    }
}