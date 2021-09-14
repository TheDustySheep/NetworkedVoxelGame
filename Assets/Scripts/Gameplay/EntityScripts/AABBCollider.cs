using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts.Client;

namespace Scripts.Gameplay.EntityScripts
{
    public class AABBCollider : BoxCollider
    {
        /*
        [SerializeField] Bounds bounds;
        private Bounds worldCheckBounds = new Bounds(Vector3.zero, Vector3.one);
        private static Dimension dimension;

        private void FixedUpdate()
        {
            bounds.center = transform.position;

            if (dimension == null)
            {
                dimension = FindObjectOfType<Dimension>();

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            IsColliding = true;
                            Colisions[x + 1, y + 1, z + 1] = true;
                        }
                    }
                }
            }
            else
            {
                bool isColliding = false;

                Vector3Int pos = GetPosition(bounds.center);

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        for (int z = -1; z < 2; z++)
                        {
                            Vector3Int _pos = new Vector3Int(x, y, z) + pos;
                            var type = dimension.GetVoxelType(_pos);

                            if (type == null)
                                Colisions[x + 1, y + 1, z + 1] = false;
                            else if (type.UseCollider)
                            {
                                worldCheckBounds.center = _pos + (Vector3.one * 0.5f);

                                bool collides = bounds.Intersects(worldCheckBounds);

                                bounds.

                                if (collides)
                                    isColliding = true;

                                Colisions[x + 1, y + 1, z + 1] = collides;
                            }
                            else
                                Colisions[x + 1, y + 1, z + 1] = false;
                        }
                    }
                }

                IsColliding = isColliding;
            }
        }
        */
    }
}