using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scripts.Gameplay.WorldScripts.Server;

namespace Scripts.CustomEditors.WorldScripts
{
    [CustomEditor(typeof(ServerDimension))]
    public class ServerDimensionEditor : Editor
    {
        ServerDimension dimension;

        private void OnEnable()
        {
            dimension = (ServerDimension)target;    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUILayout.Label("Chunks", EditorStyles.boldLabel);
            GUILayout.Label($"Active Chunks: {dimension.ActiveChunks}");
            GUILayout.Label($"Spawn Queue: {dimension.ChunksToSpawn}");
            GUILayout.Label($"Client Requests: {dimension.ClientRequestCount}");
            GUILayout.Label($"Update All: {dimension.UpdateAllRequestCount}");

            GUILayout.Space(10f);
            GUILayout.Label("Terrain Generation", EditorStyles.boldLabel);
            GUILayout.Label($"Average Time: {((int)(dimension.AverageSpawnTime * 1000f)) / 1000f} ms");
            GUILayout.Label($"Total Time: {dimension.TotalSpawnTime} ms");
            GUILayout.Label($"Total Spawns: {dimension.TotalChunkSpawns}");

            EditorUtility.SetDirty(dimension);
        }
    }
}