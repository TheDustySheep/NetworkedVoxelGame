using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Gameplay.InventoryScripts;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Scripts.UI.InventoryScripts
{
    public class UIItemSlot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] GameObject imagePrefab;

        Action<PointerEventData, UIItemSlot> callback;

        public void OnPointerClick(PointerEventData eventData)
        {
            callback?.Invoke(eventData, this);
        }

        public void Initilize(Action<PointerEventData, UIItemSlot> _callback, ItemSlot itemSlot)
        {
            callback = _callback;

            if (!itemSlot.IsEmpty)
            {
                Sprite[] sprites = itemSlot.item.GetSprites();

                for (int i = 0; i < sprites.Length; i++)
                {
                    Instantiate(imagePrefab, transform).GetComponent<Image>().sprite = sprites[i];
                }
            }
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