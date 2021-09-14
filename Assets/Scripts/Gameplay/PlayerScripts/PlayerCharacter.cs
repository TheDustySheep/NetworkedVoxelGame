using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.PlayerScripts
{
    public class PlayerCharacter : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] Transform bodyTransform;
        [SerializeField] Transform localOnlyTransform;
        [SerializeField] Transform otherOnlyTransform;
        [SerializeField] Rigidbody rb;

        [Space]
        [Header("Starting Conditions")]
        [SerializeField] Vector3 startPos;

        [Space]
        [Header("Fall out of world")]
        [SerializeField] float respawnHeight = 128f;
        [SerializeField] float minHeight = -128f;

        [Space]
        [Header("Scriptable References")]
        [SerializeField] Utils.ScriptableReference.Vector3Reference playerPosition;

        private void Start()
        {
            bodyTransform.position = startPos;

            localOnlyTransform.gameObject.SetActive(hasAuthority);
            otherOnlyTransform.gameObject.SetActive(!hasAuthority);
        }

        private void Update()
        {
            if (!hasAuthority) return;

            Vector3 position = bodyTransform.position;

            playerPosition.value = position;

            if (position.y < minHeight)
            {
                bodyTransform.position = new Vector3(position.x, respawnHeight, position.z);
                rb.velocity = Vector3.zero;
            }
        }
    }
}