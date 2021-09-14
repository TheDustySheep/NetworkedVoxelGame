using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public static class RequiredChunkManager
    {
        public static HashSet<ChunkPos> GetChunksSphere(ChunkPos centre, int radius)
        {
            HashSet<ChunkPos> chunks = new HashSet<ChunkPos>();

            for (int x = -radius; x < 1 + radius; x++)
            {
                for (int y = -radius; y < 1 + radius; y++)
                {
                    for (int z = -radius; z < 1 + radius; z++)
                    {
                        if (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) < radius + 1)
                            chunks.Add(new Vector3Int(x, y, z) + centre);
                    }
                }
            }

            return chunks;
        }

        public static HashSet<ChunkPos> GetChunksCylinder(ChunkPos centre, int radius, Vector2Int worldHeights)
        {
            HashSet<ChunkPos> chunks = new HashSet<ChunkPos>();

            for (int x = -radius; x < 1 + radius; x++)
            {
                for (int z = -radius; z < 1 + radius; z++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(z) < radius + 1)
                    {
                        for (int y = worldHeights.x; y < worldHeights.y; y++)
                        {
                            chunks.Add(new Vector3Int(x, y, z) + new Vector3Int(centre.x, 0, centre.z));
                        }
                    }
                }
            }

            return chunks;
        }
    }
}