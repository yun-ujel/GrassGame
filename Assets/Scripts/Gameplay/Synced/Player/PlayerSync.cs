using UnityEngine;
using Unity.Netcode;
using GrassGame.Gameplay.Local;

namespace GrassGame.Gameplay.Synced
{
    public class PlayerSync : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalGameManager.Instance.SetLocalPlayer(this);
                gameObject.name = "Local Player";
            }
        }

        private void Update()
        {

        }

        #region Character Methods
        public void StartCharacter(Rigidbody writerBody)
        {

        }
        #endregion
    }
}