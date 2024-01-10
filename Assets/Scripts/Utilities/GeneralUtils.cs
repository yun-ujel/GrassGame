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

        public static float SqrDistance(Vector3 target, Vector3 current)
        {
            return (target - current).sqrMagnitude;
        }
    }
}
