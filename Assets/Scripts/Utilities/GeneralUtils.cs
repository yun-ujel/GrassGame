using UnityEngine;
using UnityEngine.InputSystem;

namespace GrassGame.Utilities
{
    public static class GeneralUtils
    {
        public static string PlayerInputTag { get; } = "PlayerInput";
        public static PlayerInput GetPlayerInput()
        {
            return GameObject.FindGameObjectWithTag(PlayerInputTag).GetComponent<PlayerInput>();
        }
    }
}
