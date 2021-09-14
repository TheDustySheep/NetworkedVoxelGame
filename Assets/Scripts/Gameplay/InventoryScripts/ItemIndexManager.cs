using UnityEngine;
using Scripts.Utils;
using System;
using System.Collections.Generic;

namespace Scripts.Gameplay.InventoryScripts
{
    public class ItemIndexManager : MonoBehaviour
    {
        public static ItemIndexManager Instance;

        //Items
        public Indexer<ItemType> ItemType = new Indexer<ItemType>();

        //Tools
        public Indexer<HeadType> HeadType = new Indexer<HeadType>();
        public Indexer<RodType> RodType = new Indexer<RodType>();

        private void Awake()
        {
            Initilize("Items");
            Instance = this;
        }

        public void Initilize(string itemPath)
        {
            ItemType.UpdateIndex(itemPath);
            HeadType.UpdateIndex(itemPath);
            RodType.UpdateIndex(itemPath);
        }
    }
}