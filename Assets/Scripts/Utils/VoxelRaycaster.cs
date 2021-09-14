using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class VoxelRaycaster
    {
        public delegate bool CheckPos3D(Vector3Int pos);
        public static bool Cast3D(Ray ray, float range, CheckPos3D check, out VoxelHit hit)
        {
            //List<(Vector3, Vector3Int)> checkedPoints = new List<(Vector3, Vector3Int)>();
            Vector3 Curr = ray.origin; //Current
            Vector3 Dir = ray.direction.normalized; //Derivitive
            Vector3 iDir = Vector3.zero; //Inverse Derivitive
            float distTraveled = 0f;

            iDir.x = Dir.x == 0 ? 1000000000000f : 1f / Dir.x;
            iDir.y = Dir.y == 0 ? 1000000000000f : 1f / Dir.y;
            iDir.z = Dir.z == 0 ? 1000000000000f : 1f / Dir.z;

            bool xNeg = Dir.x < 0;
            bool yNeg = Dir.y < 0;
            bool zNeg = Dir.z < 0;

            int i = 0;

            hit = new VoxelHit();

            while (distTraveled < range)
            {
                Vector3 Next = new Vector3(
                    xNeg ? RoundDown(Curr.x) : RoundUp(Curr.x),
                    yNeg ? RoundDown(Curr.y) : RoundUp(Curr.y),
                    zNeg ? RoundDown(Curr.z) : RoundUp(Curr.z));

                Vector3 Step = Next - Curr;
                Vector3 Dist = new Vector3(Step.x * iDir.x, Step.y * iDir.y, Step.z * iDir.z);
                Vector3 CurrStep;
                Vector3 normal;
                float CurrStepDist;

                bool xMin = (Dist.x < Dist.y) && (Dist.x < Dist.z);
                bool yMin = (Dist.y < Dist.x) && (Dist.y < Dist.z);

                if (xMin)
                {
                    CurrStep = Dist.x * Dir;
                    CurrStepDist = Dist.x;
                    normal = xNeg ? Vector3.right : Vector3.left;
                }
                else if (yMin)
                {
                    CurrStep = Dist.y * Dir;
                    CurrStepDist = Dist.y;
                    normal = yNeg ? Vector3.up : Vector3.down;
                }
                else
                {
                    CurrStep = Dist.z * Dir;
                    CurrStepDist = Dist.z;
                    normal = zNeg ? Vector3.forward : Vector3.back;
                }

                if (distTraveled + CurrStepDist < range)
                {
                    Curr += CurrStep;
                    distTraveled += CurrStepDist;
                }

                Vector3 _stepped = Curr + (Dir * 0.00001f);

                Vector3Int hitVoxel = Vector3Int.zero;
                hitVoxel.x = Mathf.FloorToInt(_stepped.x);
                hitVoxel.y = Mathf.FloorToInt(_stepped.y);
                hitVoxel.z = Mathf.FloorToInt(_stepped.z);

                if (check.Invoke(hitVoxel))
                {
                    hit.Position = Curr;
                    hit.VoxelPosition = hitVoxel;
                    hit.Normal = normal;
                    hit.Distance = distTraveled;
                    return true;
                }

                i++;

                if (i > 50)
                    break;
            }

            return false;
        }

        private static float RoundUp(float input)
        {
            float output = Mathf.Ceil(input);
            output = output == input ? output + 1f : output;
            return output;
        }

        private static float RoundDown(float input)
        {
            float output = Mathf.Floor(input);
            output = output == input ? output - 1f : output;
            return output;
        }
    }

    public struct VoxelHit
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3Int VoxelPosition;
        public float Distance;
    }
}