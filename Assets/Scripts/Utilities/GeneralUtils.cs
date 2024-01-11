using UnityEngine;
using UnityEngine.EventSystems;
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

        public static void UIFocus(this EventSystem eventSystem, GameObject gameObjectToFocus)
        {
            eventSystem.enabled = false;
            eventSystem.sendNavigationEvents = false;
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(gameObjectToFocus);
            eventSystem.sendNavigationEvents = true;
            eventSystem.enabled = true;
        }
    }
}
