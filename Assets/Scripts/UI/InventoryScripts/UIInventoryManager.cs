using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Scripts.Gameplay.InventoryScripts;
using UnityEngine.EventSystems;

namespace Scripts.UI.InventoryScripts
{
    public class UIInventoryManager : MonoBehaviour
    {
        [SerializeField] HandSlotManager handSlotManager;
        [SerializeField] GameObject UIInventoryPrefab;

        Dictionary<UIInventory, Inventory> inventories = new Dictionary<UIInventory, Inventory>();

        private void Awake()
        {
            if (handSlotManager == null)
            {
                handSlotManager = FindObjectOfType<HandSlotManager>();
            }
        }

        private void OnEnable()
        {
            Signals.Get<OpenInventorySignal>().AddListener(OnInventoryOpen);
            Signals.Get<UpdateInventorySignal>().AddListener(OnInventoryUpdate);
            Signals.Get<CloseInventorySignal>().AddListener(OnInventoryClose);
        }

        private void OnDisable()
        {
            Signals.Get<OpenInventorySignal>().RemoveListener(OnInventoryOpen);
            Signals.Get<UpdateInventorySignal>().RemoveListener(OnInventoryUpdate);
            Signals.Get<CloseInventorySignal>().RemoveListener(OnInventoryClose);
        }

        private void OnInventoryOpen(Inventory inventory)
        {
            var go = Instantiate(UIInventoryPrefab, transform);
            var UIInv = go.GetComponent<UIInventory>();
            UIInv.InitilizeInventory(OnSlotClicked, inventory);

            inventories.Add(UIInv, inventory);
        }

        private void OnInventoryUpdate(Inventory inventory)
        {
            if (!inventories.ContainsValue(inventory))
                return;

            foreach (var item in inventories)
            {
                if (item.Value == inventory)
                {
                    item.Key.UpdateInventory(inventory);
                }
            }
        }

        private void OnInventoryClose(Inventory inventory)
        {
            if (!inventories.ContainsValue(inventory))
                return;

            UIInventory uIInventory = null;
            foreach (var item in inventories)
            {
                if (item.Value == inventory)
                {
                    uIInventory = item.Key;
                    break;
                }
            }

            if (uIInventory != null)
            {
                inventories.Remove(uIInventory);
                Destroy(uIInventory.gameObject);
            }
        }

        private void OnSlotClicked(UIInventory uIInventory, PointerEventData eventData, int index)
        {
            if (inventories.TryGetValue(uIInventory, out var inventory))
            {
                if (inventory == null)
                {
                    inventories.Remove(uIInventory);
                    return;
                }

                handSlotManager?.OnSlotClicked(eventData, inventory, index);
            }
        }
    }
}