using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts
{
    [CreateAssetMenu(menuName = "World/Rendering Settings")]
    public class ChunkRenderingSettings : ScriptableObject
    {
        public ComputeShader shader;
        public Material solidMaterial;
        public Material transparentMaterial;
        public Material[] materials;
        public int layer;
        public Vector2Int uvCount = new Vector2Int(16, 16);

        public bool enableMeshCollider = true;
    }
}