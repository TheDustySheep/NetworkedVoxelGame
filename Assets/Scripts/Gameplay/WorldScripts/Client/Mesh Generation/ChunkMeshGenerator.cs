using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Scripts.Utils;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public class ChunkMeshGenerator
    {
        private static readonly int SubMeshCount = 2;

        private static readonly int shaderVoxelDataSize     = Marshal.SizeOf(typeof(ShaderVoxelData));
        private static readonly int shaderVoxelTypeDataSize = Marshal.SizeOf(typeof(ShaderVoxelTypeData));

        ChunkRenderingSettings settings;
        VoxelIndexer indexer;
        ComputeShader shader;

        public ChunkMeshGenerator(ChunkRenderingSettings _settings, VoxelIndexer _indexer)
        {
            indexer = _indexer;
            settings = _settings;
            shader = settings.shader;
        }

        public IEnumerator Regenerate(ShaderVoxelData[] chunkData, ShaderVoxelData[] neighbourData, FaceData faceData)
        {
            //Setup
            int kernelIndex = shader.FindKernel("CSMain");

            //Create Buffers
            ComputeBuffer faceBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3_6, sizeof(float) * ((4 * 3) + (4 * 2) + 1), ComputeBufferType.Append);
            ComputeBuffer faceCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            ComputeBuffer voxelDataBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3, shaderVoxelDataSize);
            ComputeBuffer voxelNeighbourDataBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3_6, shaderVoxelDataSize);
            ComputeBuffer voxelTypesBuffer = new ComputeBuffer(indexer.LoadedCount + 1, shaderVoxelTypeDataSize);

            //Set Buffer Datas
            voxelDataBuffer.SetData(chunkData);
            voxelNeighbourDataBuffer.SetData(neighbourData);
            voxelTypesBuffer.SetData(indexer.shaderVoxelTypes);

            //Send Values
            faceBuffer.SetCounterValue(0);
            shader.SetFloat("uvIncrementX", 1f / settings.uvCount.x);
            shader.SetFloat("uvIncrementY", 1f / settings.uvCount.y);

            //Send Buffers
            shader.SetBuffer(kernelIndex, "faces", faceBuffer);
            shader.SetBuffer(kernelIndex, "data", voxelDataBuffer);
            shader.SetBuffer(kernelIndex, "neighbourData", voxelNeighbourDataBuffer);
            shader.SetBuffer(kernelIndex, "types", voxelTypesBuffer);

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //Execute Shader
            shader.Dispatch(kernelIndex, StaticWorldData.THREAD_GROUPS_XZ, StaticWorldData.THREAD_GROUPS_Y, StaticWorldData.THREAD_GROUPS_XZ);

            //Retrieve Data
            ComputeBuffer.CopyCount(faceBuffer, faceCountBuffer, 0);
            int[] faceCountArray = { 0 };
            faceCountBuffer.GetData(faceCountArray);
            int numFaces = faceCountArray[0];

            faceData.Faces = new Face[numFaces];
            faceBuffer.GetData(faceData.Faces, 0, 0, numFaces);

            sw.Stop();
            Debug.Log($"Shader Took: {sw.ElapsedMilliseconds} ms");

            //Release Buffers
            faceBuffer.Release();
            faceCountBuffer.Release();
            voxelDataBuffer.Release();
            voxelNeighbourDataBuffer.Release();
            voxelTypesBuffer.Release();
            yield break;
        }

        public MeshData GenerateMeshData(FaceData faceData)
        {
            int numFaces = faceData.Faces.Length;

            //Create mesh arrays
            MeshData meshData = new MeshData(numFaces);

            int[] subMeshTotalTrianglesCount = new int[SubMeshCount];
            int[] triangleIndexes = new int[SubMeshCount];

            int vertCount = 0;

            for (int i = 0; i < numFaces; i++)
            {
                Face face = faceData.Faces[i];
                meshData.verts[vertCount + 0] = face.vertA;
                meshData.verts[vertCount + 1] = face.vertB;
                meshData.verts[vertCount + 2] = face.vertC;
                meshData.verts[vertCount + 3] = face.vertD;

                meshData.uvs[vertCount + 0] = face.uvsA;
                meshData.uvs[vertCount + 1] = face.uvsB;
                meshData.uvs[vertCount + 2] = face.uvsC;
                meshData.uvs[vertCount + 3] = face.uvsD;

                vertCount += 4;

                subMeshTotalTrianglesCount[face.SubMesh] += 6;
            }

            for (int i = 0; i < SubMeshCount; i++)
            {
                meshData.triangles[i] = new int[subMeshTotalTrianglesCount[i]];
            }

            vertCount = 0;

            for (int i = 0; i < numFaces; i++)
            {
                Face face = faceData.Faces[i];

                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 0] = vertCount + 0;
                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 1] = vertCount + 1;
                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 2] = vertCount + 2;
                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 3] = vertCount + 0;
                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 4] = vertCount + 2;
                meshData.triangles[face.SubMesh][triangleIndexes[face.SubMesh] + 5] = vertCount + 3;

                triangleIndexes[face.SubMesh] += 6;

                vertCount += 4;
            }

            return meshData;
        }

        public void ApplyMeshData(Mesh mesh, MeshData meshData)
        {
            mesh.Clear();
            mesh.subMeshCount = SubMeshCount;
            mesh.SetVertices(meshData.verts);
            mesh.SetUVs(0, meshData.uvs);

            for (int i = 0; i < SubMeshCount; i++)
            {
                mesh.SetTriangles(meshData.triangles[i], i);
            }

            mesh.RecalculateNormals();
        }

        public class FaceData
        {
            public Face[] Faces;
        }

        public struct Face
        {
            public int SubMesh;

            public Vector3 vertA;
            public Vector3 vertB;
            public Vector3 vertC;
            public Vector3 vertD;

            public Vector2 uvsA;
            public Vector2 uvsB;
            public Vector2 uvsC;
            public Vector2 uvsD;
        };

        public class MeshData
        {
            public Vector3[] verts;
            public Vector2[] uvs;
            public int[][] triangles;

            public MeshData(int numFaces)
            {
                verts = new Vector3[numFaces * 4];
                uvs = new Vector2[numFaces * 4];
                triangles = new int[SubMeshCount][];
            }
        };
    }
}