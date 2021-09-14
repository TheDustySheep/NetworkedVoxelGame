using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scripts.Utils.ScriptableReference;

namespace Scripts.UI
{
    public class DebugOverlay : MonoBehaviour
    {
        [SerializeField] TMP_Text textBox;
        [SerializeField] Vector3Reference playerPosition;
        [SerializeField] float refreshPerSecond = 8;

        private void Start()
        {
            StartCoroutine(UpdateText());
        }

        private IEnumerator UpdateText()
        {
            WaitForSeconds wait = new WaitForSeconds(1f / refreshPerSecond);
            while (true)
            {
                if (textBox != null && playerPosition != null)
                {
                    Vector3 pos = playerPosition.value;
                    textBox.text = $"Position\nX: {Mathf.Floor(pos.x)}\nY: {Mathf.Floor(pos.y)}\nZ: {Mathf.Floor(pos.z)}";
                }

                yield return wait;
            }
        }
    }
}