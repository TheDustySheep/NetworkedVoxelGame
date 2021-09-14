using Scripts.Gameplay.WorldScripts.PhysicsScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.CustomEditors.WorldScripts
{
    [CustomEditor(typeof(PhysicsDimension))]
    public class PhysicsDimensionEditor : Editor
    {
        PhysicsDimension dimension;

        private void OnEnable()
        {
            dimension = (PhysicsDimension)target;    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10f);
            GUILayout.Label("Physics Chunks", EditorStyles.boldLabel);
            GUILayout.Label($"Active Chunks: {dimension.ActiveChunkCount}");
            GUILayout.Label($"Inactive Chunks: {dimension.InactiveChunkCount}");
        }
    }
}