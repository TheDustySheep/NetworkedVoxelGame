using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts;
using Scripts.Gameplay.WorldScripts.Client;
using Mirror;
using Scripts.Utils;

namespace Scripts.Gameplay.PlayerScripts
{
    public class PlayerVoxelEdit : NetworkBehaviour
    {
        [SerializeField] LayerMask layer;
        [SerializeField] float editReach = 10f;
        [SerializeField] Camera cam;
        [SerializeField] GameObject highlightPrefab;
        [HideInInspector] ClientDimension clientDimension;

        GameObject highlightCube;

        VoxelHit lastHit;

        private void Awake()
        {
            if (!hasAuthority)
                return;

            highlightCube = Instantiate(highlightPrefab, transform);
        }

        private void Update()
        {
            if (!hasAuthority)
                return;

            if (clientDimension == null)
            {
                clientDimension = FindObjectOfType<ClientDimension>();

                if (clientDimension == null)
                    return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (VoxelRaycaster.Cast3D(ray, editReach, IsSolid, out lastHit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    VoxelData voxelData = new VoxelData("Air");
                    clientDimension?.RequestEdit(lastHit.VoxelPosition, voxelData);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    VoxelData voxelData = new VoxelData("Grass");
                    clientDimension?.RequestEdit(lastHit.VoxelPosition, voxelData);
                }

                if (highlightCube != null)
                {
                    highlightCube.SetActive(true);
                    highlightCube.transform.position = lastHit.VoxelPosition + (Vector3.one * 0.5f);
                }
            }
            else
            {
                if (highlightCube != null)
                {
                    highlightCube.SetActive(false);
                }
            }
        }

        private bool IsSolid(Vector3Int pos)
        {
            var type = clientDimension.GetVoxelType(pos);
            if (type == null)
                return true;
            else
                return type.UseCollider;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHit.Position, 0.05f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lastHit.VoxelPosition, 0.05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(lastHit.Position, lastHit.Position + lastHit.Normal);
        }
    }
}