using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class ChunkRendererGraphicsDraw : IChunkRenderer
    {
        Matrix4x4 matrix;
        Mesh mesh;
        ChunkRenderingSettings settings;

        public ChunkRendererGraphicsDraw(Vector3Int chunkPos, ChunkRenderingSettings _settings, Mesh _mesh)
        {
            mesh = _mesh;
            settings = _settings;
            matrix = Matrix4x4.TRS(chunkPos * StaticWorldData.CHUNK_SIZE, Quaternion.identity, Vector3.one);
        }

        public void Draw()
        {
            if (mesh == null)
                return;

            Graphics.DrawMesh(mesh, matrix, settings.solidMaterial, settings.layer);
        }

        public IEnumerator OnRegenerate()
        {
            yield break;
        }

        public void Destroy() { }
    }
}