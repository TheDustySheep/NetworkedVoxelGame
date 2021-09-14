using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    [CreateAssetMenu(menuName = "Items/Tools/Rod Type")]
    public class RodType : ItemType
    {
        public RodMaterial rodMaterial;
        public float Range = 1f;
        public float energyUse = 1f;
    }
}