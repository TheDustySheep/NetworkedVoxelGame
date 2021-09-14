using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Scripts.Gameplay.WorldScripts.Client;

namespace Scripts.Gameplay.EntityScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float GravityMultiplier = 1f;
        static ClientDimension dimension;
        VoxelHit[] lastHits = new VoxelHit[10];
        Rigidbody rb;

        [SerializeField] Transform groundCheck;
        [SerializeField] Transform headCheck;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }

        private void FixedUpdate()
        {
            Vector3 nextVelocity = Vector3.zero;

            if (dimension == null)
            {
                dimension = FindObjectOfType<ClientDimension>();

                nextVelocity = Vector3.zero;
            }
            else
            {
                nextVelocity = rb.velocity;
                nextVelocity += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;

                //Down Check
                if (CheckDistance(Vector3.down, groundCheck.position, 2f, ref lastHits[0]))
                {
                    if (lastHits[0].Distance < 0.1f)
                    {
                        nextVelocity.y = 0f;
                    }
                    else
                    {
                        nextVelocity.y = Mathf.Clamp(nextVelocity.y, -2f, 2f);
                    }
                }

                //Up Check
                if (CheckDistance(Vector3.up, headCheck.position, 2f, ref lastHits[1]))
                {
                    if (lastHits[1].Distance < 0.1f)
                    {
                        nextVelocity.y = 0f;
                    }
                    else
                    {
                        nextVelocity.y = Mathf.Clamp(nextVelocity.y, -2f, 2f);
                    }
                }
            }

            rb.velocity = nextVelocity;
        }

        private bool CheckDistance(Vector3 normal, Vector3 position, float checkDist, ref VoxelHit hit)
        {
            if (VoxelRaycaster.Cast3D(new Ray(position, (position + normal).normalized), 2f, IsSolid, out hit))
            {
                if (hit.Distance < checkDist)
                    return true;
            }

            return false;
        }

        private bool IsSolid(Vector3Int pos) 
        {
            var type = dimension.GetVoxelType(pos);
            if (type == null)
                return true;
            else
                return type.UseCollider;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < 10; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(lastHits[i].Position, 0.05f);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(lastHits[i].VoxelPosition, 0.05f);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(lastHits[i].Position, lastHits[i].Position + lastHits[i].Normal);
            }
        }
    }
}