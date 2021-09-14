using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    [CreateAssetMenu(menuName = "Items/Item Type")]
    public class ItemType : ScriptableObject
    {
        public string ItemDisplayName => itemDisplayName == string.Empty ? name : itemDisplayName;
        [SerializeReference] string itemDisplayName;

        public Sprite sprite;
        public int stackSize = 1;
    }
}