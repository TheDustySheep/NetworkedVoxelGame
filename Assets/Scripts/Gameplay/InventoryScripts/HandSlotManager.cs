using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Gameplay.InventoryScripts
{
    public class HandSlotManager : MonoBehaviour
    {
        ItemSlot handSlot;
        
        public void OnSlotClicked(PointerEventData eventData, Inventory inventory, int index)
        {
            Debug.Log($"Slot Clicked {index}");

            if (inventory.itemSlots.Length <= index)
                return;

            if (handSlot.IsEmpty)
            {
                if (inventory.itemSlots[index].IsEmpty)
                {
                    return;
                }
                else
                {
                    handSlot = inventory.itemSlots[index];
                    inventory.itemSlots[index].Clear();
                }
            }
            else
            {
                if (inventory.itemSlots[index].IsEmpty)
                {
                    inventory.itemSlots[index] = handSlot;
                    handSlot.Clear();
                }
                else
                {
                    ItemSlot hand = handSlot;
                    ItemSlot inv = inventory.itemSlots[index];

                    handSlot = inv;
                    inventory.itemSlots[index] = hand;
                }
            }

            inventory.OnItemChange();
            Utils.Signals.Get<HandSlotUpdateSignal>().Dispatch(handSlot);
        }
    }
}