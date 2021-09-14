using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client.MeshGeneration
{
    public class MeshObjectDrawMesh
    {
        public Mesh mesh;

        Material[] materials;
        Matrix4x4 matrix;
        int layer;

        public MeshObjectDrawMesh(ChunkPos chunkPos, ChunkRenderingSettings chunkRenderingSettings)
        {
            //Copy Data
            materials = chunkRenderingSettings.materials;
            layer = chunkRenderingSettings.layer;

            //Create Matrix
            matrix = Matrix4x4.TRS((Vector3Int)chunkPos * StaticWorldData.CHUNK_SIZE, Quaternion.identity, Vector3.one);

            //Create Mesh
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        }

        public void Draw()
        {
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                Graphics.DrawMesh(mesh, matrix, materials[i], layer, null, i);
            }
        }
    }
}