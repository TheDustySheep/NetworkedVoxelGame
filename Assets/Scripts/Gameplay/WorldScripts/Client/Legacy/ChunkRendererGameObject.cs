using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.ClientLegacy
{
    public class ChunkRendererGameObject : IChunkRenderer
    {
        Mesh tMesh;
        Mesh sMesh;

        GameObject tgo;
        GameObject sgo;

        MeshCollider scollider;
        MeshCollider tcollider;

        public ChunkRendererGameObject(Vector3Int chunkPos, ChunkRenderingSettings _settings, Mesh _Smesh, Mesh _Tmesh, Transform parent)
        {
            tMesh = _Tmesh;
            sMesh = _Smesh;

            tgo = new GameObject();
            tgo.transform.name = $"Chunk Transparent: {chunkPos}";
            tgo.transform.SetParent(parent);
            tgo.transform.localPosition = chunkPos * StaticWorldData.CHUNK_SIZE;
            tgo.AddComponent<MeshFilter>().sharedMesh = _Tmesh;
            tgo.AddComponent<MeshRenderer>().sharedMaterial = _settings.solidMaterial;

            sgo = new GameObject();
            sgo.transform.name = $"Chunk Solid: {chunkPos}";
            sgo.transform.SetParent(parent);
            sgo.transform.localPosition = chunkPos * StaticWorldData.CHUNK_SIZE;
            sgo.AddComponent<MeshFilter>().sharedMesh = _Smesh;
            sgo.AddComponent<MeshRenderer>().sharedMaterial = _settings.solidMaterial;

            if (_settings.enableMeshCollider)
            {
                scollider = sgo.AddComponent<MeshCollider>();
                tcollider = tgo.AddComponent<MeshCollider>();
            }
        }

        public void Destroy()
        {
            Object.Destroy(tgo);
            Object.Destroy(sgo);
        }

        public void Draw() { }

        public IEnumerator OnRegenerate()
        {
            int smeshID = tMesh.GetInstanceID();
            int tmeshID = sMesh.GetInstanceID();

            yield return new Utils.WaitForThreadedTask(() =>
            {
                Physics.BakeMesh(smeshID, false);
                Physics.BakeMesh(tmeshID, false);
            });

            if (scollider != null)
                scollider.sharedMesh = sMesh;

            if (tcollider != null)
                tcollider.sharedMesh =tMesh;
        }
    }
}