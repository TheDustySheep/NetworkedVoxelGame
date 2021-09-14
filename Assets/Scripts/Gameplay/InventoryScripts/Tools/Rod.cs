using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.InventoryScripts
{
    public class Rod
    {
        public int HashKey;
        public RodType Type => ItemIndexManager.Instance.RodType.GetItem(HashKey);

        public float Quality;

        public Rod(RodType _rodType, float _quality)
        {
            HashKey = _rodType.name.GetStableHashCode();
            Quality = _quality;
        }

        public Rod(RodType _rodType)
        {
            HashKey = _rodType.name.GetStableHashCode();
            Quality = Random.Range(0f, 1f);
        }
    }
}