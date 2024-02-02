using UnityEngine;
using Unity.Netcode;

using GrassGame.Gameplay.Local;
using GrassGame.Gameplay.Synced.Player.Enumerations;
using GrassGame.Gameplay.Synced.Player.Data;
using GrassGame.Gameplay.Synced.Player.Readers;

namespace GrassGame.Gameplay.Synced
{
    public class PlayerSync : NetworkBehaviour
    {
        #region Parameters
        public NetworkVariable<PlayerType> networkPlayerType = new(PlayerType.NotSet, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        #region Character
        private NetworkVariable<CharacterMovementData> characterMovementData = new(CharacterMovementData.Zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 Position => characterMovementData.Value.Position;

        private Rigidbody body;
        #endregion
        #endregion

        public override void OnNetworkSpawn()
        {
            RegisterToGameManager();
        }

        private void RegisterToGameManager()
        {
            if (IsOwner)
            {
                LocalGameManager.Instance.SetLocalPlayer(this);
                gameObject.name = "Local Player";
                return;
            }

            LocalGameManager.Instance.AddSyncedPlayer(this);
            gameObject.name = "Synced Player";
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (networkPlayerType.Value == PlayerType.Character)
            {
                TransmitCharacterMovement();
            }
        }

        #region Character Methods
        public void StartCharacter(GameObject character)
        {
            networkPlayerType.Value = PlayerType.Character;
            if (IsOwner)
            {
                body = character.GetComponent<Rigidbody>();
                return;
            }

            characterMovementData.OnValueChanged += character.GetComponent<CharacterMovementReader>().OnMovementDataChanged;
            return;
        }

        private void TransmitCharacterMovement()
        {
            characterMovementData.Value = new CharacterMovementData(body.velocity, Vector3.zero, body.position);
        }

        public void OnTypeSet(PlayerType notSet, PlayerType type)
        {
            networkPlayerType.OnValueChanged -= OnTypeSet;
            LocalGameManager.Instance.OnTypeSet(this);
        }
        #endregion
    }
}