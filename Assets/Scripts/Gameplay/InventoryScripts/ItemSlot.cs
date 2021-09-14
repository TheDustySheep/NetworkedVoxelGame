using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.InventoryScripts
{
    [System.Serializable]
    public struct ItemSlot
    {
        public Item item;
        public int amount;

        public bool IsEmpty => item == null || amount == 0;

        public ItemSlot(Item _item, int _amount)
        {
            item = _item;
            amount = _amount;
        }

        public int Add(Item _item, int _amount)
        {
            if (item == null)
            {
                item = _item;
                amount = _amount;
                return 0;
            }

            if (item != _item)
            {
                return _amount;
            }

            amount += _amount;
            return 0;
        }

        public void Clear()
        {
            item = null;
            amount = 0;
        }

        public ItemSlot Clone()
        {
            return new ItemSlot()
            {
                item = item,
                amount = amount
            };
        }
    }
}