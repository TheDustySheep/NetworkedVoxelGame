using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.InventoryScripts
{
    public class InventoryHolder : MonoBehaviour
    {
        [SerializeField] HeadType headType;
        [SerializeField] RodType rodType;

        [SerializeField] ItemType itemType;
        [SerializeField] Inventory inventory;
        Indexer<ItemType> indexer = new Utils.Indexer<ItemType>();

        private void Start()
        {
            indexer.UpdateIndex("Items");
            inventory = new Inventory(20);

            Item item = new Item() { HashKey = indexer.GetHash(itemType) };

            ItemSlot itemSlot = new ItemSlot(item, 1);

            for (int i = 0; i < 5; i++)
            {
                inventory.AddItem(itemSlot);
            }

            Tool tool = ToolFactory.Generate(headType, rodType);

            itemSlot = new ItemSlot(tool, 1);

            for (int i = 0; i < 5; i++)
            {
                inventory.AddItem(itemSlot);
            }

            Signals.Get<OpenInventorySignal>().Dispatch(inventory);
        }
    }
}