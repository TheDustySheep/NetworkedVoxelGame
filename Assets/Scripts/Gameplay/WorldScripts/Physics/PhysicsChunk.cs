using Scripts.Utils;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.PhysicsScripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class PhysicsChunk : MonoBehaviour
    {
        private static readonly int shaderVoxelDataSize = Marshal.SizeOf(typeof(ShaderVoxelData));

        [SerializeField] ComputeShader shader;

        Mesh mesh;
        MeshCollider meshCollider;

        private void Awake()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }

        public void UpdateMesh(Indexer<VoxelType> indexer, ChunkPos chunkPos, ChunkData chunkData)
        {
            StartCoroutine(MeshUpdate(indexer, chunkPos, chunkData));
        }

        private IEnumerator MeshUpdate(Indexer<VoxelType> indexer, ChunkPos chunkPos, ChunkData chunkData)
        {
            ShaderVoxelData[] data = new ShaderVoxelData[StaticWorldData.CHUNK_SIZE_3];

            yield return new WaitForThreadedTask(() =>
            {
                for (int i = 0; i < StaticWorldData.CHUNK_SIZE_3; i++)
                {
                    data[i] = new ShaderVoxelData(indexer, chunkData.data[i]);
                }
            });

            //Setup
            int kernelIndex = shader.FindKernel("CSMain");

            //Create Buffers
            ComputeBuffer faceBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3_6, sizeof(float) * 4 * 3, ComputeBufferType.Append);
            ComputeBuffer faceCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            ComputeBuffer voxelDataBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3, shaderVoxelDataSize);

            //Set Buffer Datas
            voxelDataBuffer.SetData(data);

            //Send Values
            faceBuffer.SetCounterValue(0);

            //Send Buffers
            shader.SetBuffer(kernelIndex, "faces", faceBuffer);
            shader.SetBuffer(kernelIndex, "data", voxelDataBuffer);

            //Execute Shader
            shader.Dispatch(kernelIndex, StaticWorldData.THREAD_GROUPS_XZ, StaticWorldData.THREAD_GROUPS_Y, StaticWorldData.THREAD_GROUPS_XZ);

            yield return null;

            //Retrieve Data
            ComputeBuffer.CopyCount(faceBuffer, faceCountBuffer, 0);
            int[] faceCountArray = { 0 };
            faceCountBuffer.GetData(faceCountArray);
            int numFaces = faceCountArray[0];

            var Faces = new Face[numFaces];
            faceBuffer.GetData(Faces, 0, 0, numFaces);

            //Release Buffers
            faceBuffer.Release();
            faceCountBuffer.Release();
            voxelDataBuffer.Release();

            //Create mesh arrays
            MeshData meshData = new MeshData(numFaces);

            yield return new WaitForThreadedTask(() =>
            {
                int vertCount = 0;
                int triangleIndex = 0;

                for (int i = 0; i < numFaces; i++)
                {
                    Face face = Faces[i];
                    meshData.verts[vertCount + 0] = face.vertA;
                    meshData.verts[vertCount + 1] = face.vertB;
                    meshData.verts[vertCount + 2] = face.vertC;
                    meshData.verts[vertCount + 3] = face.vertD;

                    meshData.triangles[triangleIndex + 0] = vertCount + 0;
                    meshData.triangles[triangleIndex + 1] = vertCount + 1;
                    meshData.triangles[triangleIndex + 2] = vertCount + 2;
                    meshData.triangles[triangleIndex + 3] = vertCount + 0;
                    meshData.triangles[triangleIndex + 4] = vertCount + 2;
                    meshData.triangles[triangleIndex + 5] = vertCount + 3;

                    vertCount += 4;
                    triangleIndex += 6;
                }
            });

            mesh.Clear();
            mesh.SetVertices(meshData.verts);
            mesh.SetTriangles(meshData.triangles, 0);

            transform.position = ((Vector3Int)chunkPos) * StaticWorldData.CHUNK_SIZE;

            int meshID = mesh.GetInstanceID();

            yield return new WaitForThreadedTask(() =>
            {
                Physics.BakeMesh(meshID, false);
            });

            meshCollider.sharedMesh = mesh;
        }

        public void Clear()
        {
            mesh.Clear();
            meshCollider.sharedMesh = mesh;
        }

        private struct Face
        {
            public Vector3 vertA;
            public Vector3 vertB;
            public Vector3 vertC;
            public Vector3 vertD;
        }

        private class MeshData
        {
            public Vector3[] verts;
            public int[] triangles;

            public MeshData(int faceCount)
            {
                verts = new Vector3[faceCount * 4];
                triangles = new int[faceCount * 6];
            }
        }
    }
}