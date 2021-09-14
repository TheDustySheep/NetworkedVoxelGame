using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

public class RaycastTest : MonoBehaviour
{
    /*
    [Header("Ray")]
    [SerializeField] Transform centre;
    [SerializeField] Transform end;

    [Header("Gizmos")]
    [SerializeField] float radius = 0.05f;
    private void OnDrawGizmos()
    {
        if (centre == null || end == null)
            return;

        Vector3 direction = end.position - centre.position;
        float range = Vector3.Distance(centre.position, end.position);

        Ray ray = new Ray(centre.position, direction.normalized);
        var points = VoxelRaycaster.Cast3D(ray, range, null, out var _);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centre.position, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end.position, radius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(centre.position, end.position);

        foreach (var posPair in points)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(posPair.Item1, radius);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(posPair.Item1, new Vector3(0f, posPair.Item1.y, posPair.Item1.z));

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(posPair.Item1, new Vector3(posPair.Item1.x, 0f, posPair.Item1.z));

            Gizmos.color = Color.white;
            Gizmos.DrawLine(posPair.Item1, new Vector3(posPair.Item1.x, posPair.Item1.y, 0f));

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(posPair.Item2, radius);
        }
    }
    */
}
