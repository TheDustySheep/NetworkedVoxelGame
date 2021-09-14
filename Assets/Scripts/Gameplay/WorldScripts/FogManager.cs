using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Client
{
    public class FogManager : MonoBehaviour
    {
        [SerializeField] float FogStartOffset = 10f;
        [SerializeField] float FogEndOffset = 20f;
        [SerializeField] ClientLegacy.ClientDimensionSettings settings;

        private void Start()
        {
            UpdateFog();
        }

        private void OnValidate()
        {
            UpdateFog();
        }

        private void UpdateFog()
        {
            int renderDistance = settings.renderDistance;
            float renderEndDistance = renderDistance * StaticWorldData.CHUNK_SIZE;

            RenderSettings.fogStartDistance = renderEndDistance - FogStartOffset;
            RenderSettings.fogEndDistance = renderEndDistance - FogEndOffset;
        }
    }
}