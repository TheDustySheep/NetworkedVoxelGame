using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class GameObjectMeshObject
    {
        public Mesh mesh;
        public GameObject go;
        public MeshCollider collider;

        public int meshID;
        public int criteria;

        public static readonly string GameObjectName = "Chunk Mesh";

        public GameObjectMeshObject(ChunkPos pos, Transform parent, Material mat, int layer, int _criteria, bool shadows)
        {
            criteria = _criteria;
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            go = new GameObject(GameObjectName);
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.material = mat;
            renderer.shadowCastingMode = shadows ? UnityEngine.Rendering.ShadowCastingMode.On : UnityEngine.Rendering.ShadowCastingMode.Off;

            if ((criteria & 2) == 2)
            {
                collider = go.AddComponent<MeshCollider>();
                UpdateCollider();
            }
            
            go.transform.parent = parent;
            go.transform.position = ((Vector3Int)pos) * StaticWorldData.CHUNK_SIZE;
            go.layer = layer;
            meshID = mesh.GetInstanceID();
        }

        public void UpdateCollider()
        {
            if (collider != null)
                collider.sharedMesh = mesh;
        }
    }
}