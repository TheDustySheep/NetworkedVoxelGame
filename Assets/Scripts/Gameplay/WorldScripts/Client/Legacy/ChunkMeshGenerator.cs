using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class ChunkMeshGenerator : IChunkMeshGenerator
    {
        private static readonly int cubeDataSize = Marshal.SizeOf(typeof(CubeData));

        ComputeBuffer faceBuffer;
        ComputeBuffer faceCountBuffer;
        ComputeBuffer cubeDataBuffer;
        ComputeBuffer cubeNeighbourDataBuffer;

        ChunkRenderingSettings settings;

        Utils.Indexer<VoxelType> indexer;

        public ChunkMeshGenerator(ChunkRenderingSettings _settings, Utils.Indexer<VoxelType> _indexer)
        {
            indexer = _indexer;
            settings = _settings;
        }

        ~ChunkMeshGenerator()
        {
            faceBuffer?.Release();
            faceCountBuffer?.Release();
            cubeDataBuffer?.Release();
            cubeNeighbourDataBuffer?.Release();
        }


        public IEnumerator Regenerate(ChunkData data, ChunkData[] neighbours, Mesh mesh, bool transparent)
        {
            //Setup
            ComputeShader shader = settings.shader;
            int kernelIndex = shader.FindKernel("CSMain");

            CubeData[] chunkData = null;
            CubeData[] neighbourData = null;

            yield return new Utils.WaitForThreadedTask(() =>
            {
                chunkData = ConvertVoxelData(data);
                neighbourData = ConvertNeighbours(neighbours);
            });

            //Create Buffers
            faceBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3_6, sizeof(float) * ((4 * 3) + (4 * 2)), ComputeBufferType.Append);
            faceCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
            cubeDataBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3, cubeDataSize);
            cubeNeighbourDataBuffer = new ComputeBuffer(StaticWorldData.CHUNK_SIZE_3_6, cubeDataSize);

            cubeDataBuffer.SetData(chunkData);
            cubeNeighbourDataBuffer.SetData(neighbourData);

            faceBuffer.SetCounterValue(0);
            shader.SetFloat("uvIncrementX", 1f / settings.uvCount.x);
            shader.SetFloat("uvIncrementY", 1f / settings.uvCount.y);
            shader.SetInt("isTransparent", transparent ? 1 : 0);
            shader.SetBuffer(kernelIndex, "faces", faceBuffer);
            shader.SetBuffer(kernelIndex, "data", cubeDataBuffer);
            shader.SetBuffer(kernelIndex, "neighbourData", cubeNeighbourDataBuffer);
            //Execute Shader
            shader.Dispatch(kernelIndex, 1, 1, 4);

            //Retrieve Data
            ComputeBuffer.CopyCount(faceBuffer, faceCountBuffer, 0);
            int[] faceCountArray = { 0 };
            faceCountBuffer.GetData(faceCountArray);
            int numFaces = faceCountArray[0];

            Face[] faces = new Face[numFaces];
            faceBuffer.GetData(faces, 0, 0, numFaces);

            Vector3[] verts = new Vector3[numFaces * 4];
            Vector2[] uvs = new Vector2[numFaces * 4];
            int[] tris = new int[numFaces * 6];

            //Release Buffers
            faceBuffer.Release();
            faceCountBuffer.Release();
            cubeDataBuffer.Release();
            cubeNeighbourDataBuffer.Release();

            yield return new Utils.WaitForThreadedTask(() =>
            {
                int vertCount = 0;
                int triCount = 0;

                for (int i = 0; i < numFaces; i++)
                {
                    Face face = faces[i];
                    verts[vertCount + 0] = face.vertA;
                    verts[vertCount + 1] = face.vertB;
                    verts[vertCount + 2] = face.vertC;
                    verts[vertCount + 3] = face.vertD;

                    uvs[vertCount + 0] = face.uvsA;
                    uvs[vertCount + 1] = face.uvsB;
                    uvs[vertCount + 2] = face.uvsC;
                    uvs[vertCount + 3] = face.uvsD;

                    tris[triCount + 0] = vertCount + 0;
                    tris[triCount + 1] = vertCount + 1;
                    tris[triCount + 2] = vertCount + 2;
                    tris[triCount + 3] = vertCount + 0;
                    tris[triCount + 4] = vertCount + 2;
                    tris[triCount + 5] = vertCount + 3;

                    vertCount += 4;
                    triCount += 6;
                }
            });

            mesh.Clear();
            mesh.SetVertices(verts);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private CubeData[] ConvertNeighbours(ChunkData[] datas)
        {
            CubeData[] cubeData = new CubeData[StaticWorldData.CHUNK_SIZE_3_6];
            ChunkData data;
            for (int i = 0; i < 6; i++)
            {
                int offset = i * StaticWorldData.CHUNK_SIZE_3;

                data = datas[i];

                if (data == null)
                {
                    for (int j = 0; j < StaticWorldData.CHUNK_SIZE_3; j++)
                    {
                        cubeData[offset + j] = CubeData.Default;
                    }
                }
                else
                {
                    for (int j = 0; j < StaticWorldData.CHUNK_SIZE_3; j++)
                    {
                        cubeData[offset + j] = indexer.GetItem(data[j].HashKey);
                    }
                }
            }

            return cubeData;
        }

        private CubeData[] ConvertVoxelData(ChunkData data)
        {
            CubeData[] cubeData = new CubeData[StaticWorldData.CHUNK_SIZE_3];

            for (int i = 0; i < StaticWorldData.CHUNK_SIZE_3; i++)
            {
                cubeData[i] = indexer.GetItem(data[i].HashKey);
            }

            return cubeData;
        }

        private struct CubeData
        {
            public int state;
            public Vector2Int topUVIndex;
            public Vector2Int sideUVIndex;
            public Vector2Int bottomUVIndex;

            public static implicit operator CubeData(VoxelType voxelType)
            {
                return new CubeData()
                {
                    state = ConvertState(voxelType),
                    topUVIndex = voxelType.yPosIndex,
                    sideUVIndex = voxelType.xPosIndex,
                    bottomUVIndex = voxelType.yNegIndex
                };
            }

            public static CubeData Default = new CubeData()
            {
                state = 0b0000_0000_0000_0010,
                topUVIndex = Vector2Int.zero,
                sideUVIndex = Vector2Int.zero,
                bottomUVIndex = Vector2Int.zero,
            };

            public static int ConvertState(VoxelType voxelType)
            {
                return
                    (voxelType.DrawSelf ? 1 : 0) |
                    (voxelType.DrawNeighboursSimilar ? 2 : 0);
            }
        };

        struct Face
        {
            public Vector3 vertA;
            public Vector3 vertB;
            public Vector3 vertC;
            public Vector3 vertD;

            public Vector2 uvsA;
            public Vector2 uvsB;
            public Vector2 uvsC;
            public Vector2 uvsD;
        };
    }
}