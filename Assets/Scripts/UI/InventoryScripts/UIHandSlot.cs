using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.InventoryScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Scripts.UI.InventoryScripts
{
    public class UIHandSlot : MonoBehaviour
    {
        [SerializeField] GameObject imagePrefab;

        private void OnEnable()
        {
            Utils.Signals.Get<HandSlotUpdateSignal>().AddListener(UpdateSlot);
        }

        private void OnDisable()
        {
            Utils.Signals.Get<HandSlotUpdateSignal>().RemoveListener(UpdateSlot);
        }

        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        public void UpdateSlot(ItemSlot itemSlot)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (!itemSlot.IsEmpty)
            {
                Sprite[] sprites = itemSlot.item.GetSprites();

                for (int i = 0; i < sprites.Length; i++)
                {
                    Instantiate(imagePrefab, transform).GetComponent<Image>().sprite = sprites[i];
                }
            }
        }
    }
}