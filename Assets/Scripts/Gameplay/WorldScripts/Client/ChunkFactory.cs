using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts.Client.MeshGeneration;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class ChunkFactory
    {
        Transform parent;
        ChunkRenderingSettings settings;

        public ChunkFactory(Transform _parent, ChunkRenderingSettings _settings)
        {
            parent = _parent;
            settings = _settings;
        }

        public Chunk SpawnChunk(ChunkPos pos, ChunkData data)
        {
            //GameObject go = new GameObject($"Chunk: {((Vector3Int)pos)}");
            //go.transform.SetParent(parent, false);
            //Transform goTransform = go.transform;

            //GameObjectMeshObject[] meshObjects = new GameObjectMeshObject[3];
            //
            //meshObjects[0] = new GameObjectMeshObject(pos, goTransform, settings.solidMaterial, settings.layer, 2, true); //Use Collider
            //meshObjects[1] = new GameObjectMeshObject(pos, goTransform, settings.transparentMaterial, settings.layer, 1, false); //Transparent
            //meshObjects[2] = new GameObjectMeshObject(pos, goTransform, settings.transparentMaterial, settings.layer, 1 | 2, false); //Transparent and collider

            MeshObjectDrawMesh meshObject = new MeshObjectDrawMesh(pos, settings);

            return new Chunk(pos, data, meshObject);
        }
    }
}