using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Gameplay.WorldScripts
{
    public class CursorLockController : MonoBehaviour
    {
        private void Update()
        {
            if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButton(0))
                {
                    Lock();
                }
            }

            if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Escape))
            {
                Unlock();
            }
        }

        private void OnDestroy()
        {
            Unlock();
        }

        private void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Unlock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}