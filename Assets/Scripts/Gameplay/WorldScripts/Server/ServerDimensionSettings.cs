using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.WorldScripts.Server
{
    [CreateAssetMenu(menuName = "World/Server Dimension Settings")]
    public class ServerDimensionSettings : ScriptableObject
    {
        public int maxActiveChunkSpawnsPerTick = 8;
        public int maxChunkUpdatesSendPerTick = 8;
    }
}