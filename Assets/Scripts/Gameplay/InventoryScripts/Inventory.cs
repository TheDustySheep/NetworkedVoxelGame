using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Gameplay.InventoryScripts
{
    public class Inventory
    {
        public ItemSlot[] itemSlots { get; private set; } = new ItemSlot[0];

        public Inventory() { }

        public Inventory(int size)
        {
            itemSlots = new ItemSlot[size];
        }

        public void AddItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].IsEmpty)
                {
                    itemSlots[i] = itemSlot;
                    return;
                }
            }
        }

        public void OnItemChange()
        {
            Utils.Signals.Get<UpdateInventorySignal>().Dispatch(this);
        }
    }
}