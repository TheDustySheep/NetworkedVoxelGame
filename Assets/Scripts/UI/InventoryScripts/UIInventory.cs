using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.InventoryScripts;
using UnityEngine.EventSystems;
using System;

namespace Scripts.UI.InventoryScripts
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] GameObject UIItemSlotPrefab;

        List<UIItemSlot> uIItemSlots = new List<UIItemSlot>();
        HashSet<UIItemSlot> uIItemSlotsHashSet = new HashSet<UIItemSlot>();

        Action<UIInventory, PointerEventData, int> slotClickedCallback;

        public void InitilizeInventory(Action<UIInventory, PointerEventData, int> callback, Inventory inventory)
        {
            slotClickedCallback = callback;

            for (int i = 0; i < uIItemSlots.Count; i++)
            {
                Destroy(uIItemSlots[i].gameObject);
            }

            uIItemSlots.Clear();
            uIItemSlotsHashSet.Clear();

            for (int i = 0; i < inventory.itemSlots.Length; i++)
            {
                UIItemSlot slot = Instantiate(UIItemSlotPrefab, transform).GetComponent<UIItemSlot>();

                uIItemSlots.Add(slot);
                uIItemSlotsHashSet.Add(slot);

                uIItemSlots[i].Initilize(OnSlotClick, inventory.itemSlots[i]);
            }
        }

        public void UpdateInventory(Inventory inventory)
        {
            for (int i = 0; i < uIItemSlots.Count; i++)
            {
                uIItemSlots[i].UpdateSlot(inventory.itemSlots[i]);
            }
        }

        private void OnSlotClick(PointerEventData eventData, UIItemSlot slot)
        {
            if (uIItemSlotsHashSet.Contains(slot))
            {
                int index = uIItemSlots.FindIndex(i => i == slot);
                slotClickedCallback?.Invoke(this, eventData, index);
            }
        }
    }
}