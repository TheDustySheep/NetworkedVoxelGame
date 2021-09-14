using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.InventoryScripts
{
    public class Item
    {
        public int HashKey;

        public virtual Sprite[] GetSprites()
        {
            return new Sprite[] 
            { 
                ItemIndexManager.Instance.ItemType.GetItem(HashKey).sprite 
            };
        }
    }
}