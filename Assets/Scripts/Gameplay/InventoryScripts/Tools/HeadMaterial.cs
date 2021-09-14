using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    [CreateAssetMenu(menuName = "Items/Tools/Head Material")]
    public class HeadMaterial : ScriptableObject
    {
        public float UseSpeedMultiplier = 1f;
        public float SharpnessWearRate = 1f;
    }
}