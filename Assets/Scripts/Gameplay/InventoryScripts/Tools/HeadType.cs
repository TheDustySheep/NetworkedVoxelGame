using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    [CreateAssetMenu(menuName = "Items/Tools/Head Type")]
    public class HeadType : ItemType
    {
        public HeadMaterial headMaterial;
        public float damageAmount = 1f;
        public bool canMine;
        public bool canShovel;
        public bool canAxe;
        public bool canFarm;
    }
}