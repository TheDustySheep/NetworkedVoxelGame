using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.WorldScripts.Client;
using UnityEditor;

namespace Scripts.CustomEditors.WorldScripts
{
    [CustomEditor(typeof(ClientDimension))]
    public class ClientDimensionEditor : Editor
    {
        ClientDimension dimension;

        private void OnEnable()
        {
            dimension = (ClientDimension)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUILayout.Label("Chunk Status", EditorStyles.boldLabel);
            GUILayout.Label($"Active Chunks: {dimension.ActiveChunkCount}");
            GUILayout.Label($"Required Chunk Count: {dimension.requiredChunkCount}");

            GUILayout.Space(10f);
            GUILayout.Label("Chunk Queues", EditorStyles.boldLabel);
            GUILayout.Label($"Needs Regeneration: {dimension.NeedRegenCount}");
            GUILayout.Label($"Awaiting Voxel Data: {dimension.VoxelDataQueueCount}");
            GUILayout.Label($"Awaiting GPU: {dimension.GPUQueueCount}");
            GUILayout.Label($"Awaiting Mesh Data: {dimension.MeshDataQueueCount}");
            GUILayout.Label($"Awaiting Apply Mesh: {dimension.ApplyMeshCount}");

            EditorUtility.SetDirty(dimension);
        }
    }
}