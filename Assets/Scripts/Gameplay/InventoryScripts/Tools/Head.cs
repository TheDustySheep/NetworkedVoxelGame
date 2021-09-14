using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Gameplay.InventoryScripts
{
    public class Head
    {
        public int HashKey;

        public HeadType Type => ItemIndexManager.Instance.HeadType.GetItem(HashKey);

        public float Sharpness;
        public float Quality;
        public float TimesSharpened;

        public Head(HeadType _headType, float _sharpness, float _quality)
        {
            HashKey = _headType.name.GetStableHashCode();
            Sharpness = _sharpness;
            Quality = _quality;
        }

        public Head(HeadType _headType)
        {
            HashKey = _headType.name.GetStableHashCode();
            Sharpness = Random.Range(0f, 1f);
            Quality = Random.Range(0f, 1f);
        }
    }
}