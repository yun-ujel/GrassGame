using Unity.Netcode;
using UnityEngine;

namespace GrassGame.Gameplay.Local.Player
{
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerIdentifier : MonoBehaviour
    {
        private void Start()
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();
            if (networkObject.IsLocalPlayer)
            {
                gameObject.name = "Local Player";
                return;
            }
            gameObject.name = "Synced Player";
        }
    }

}