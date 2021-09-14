using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Scripts.Gameplay.WorldScripts.ClientLegacy;

namespace Scripts.Gameplay.PlayerScripts
{
    public class PlayerConnection : NetworkBehaviour
    {
        [SerializeField] GameObject characterPrefab;
        [SerializeField] GameObject dimensionPrefab;

        GameObject playerCharacter;
        GameObject clientDimensionObject;

        private void Start()
        {
            if (!isLocalPlayer) return;

            CmdSpawnPlayer();
        }

        [Command]
        private void CmdSpawnPlayer()
        {
            playerCharacter = Instantiate(characterPrefab);     
            NetworkServer.Spawn(playerCharacter, connectionToClient);
            TargetSpawnPlayer(playerCharacter);
        }

        [TargetRpc]
        private void TargetSpawnPlayer(GameObject _playerConnection)
        {
            if (!isLocalPlayer) return;

            playerCharacter = _playerConnection;
            clientDimensionObject = Instantiate(dimensionPrefab);
            Dimension clientDimension = clientDimensionObject.GetComponent<Dimension>();

            if (clientDimension != null)
            {
                clientDimension.Initilize(connectionToServer);
                clientDimension.playerTransform = playerCharacter.transform;
            }
        }
    }
}