using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using GrassGame.Utilities;

namespace GrassGame.Gameplay.Local.Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerInteract : MonoBehaviour
    {
        private PlayerInput playerInput;

        [Space]

        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private SphereCollider trigger;

        List<Collider> interactablesInRange;

        private void Start()
        {
            interactablesInRange = new List<Collider>();

            playerInput = GeneralUtils.GetPlayerInput();
            SubscribeToInputActions();
        }

        private void SubscribeToInputActions()
        {
            playerInput.actions["Player/Interact"].performed += OnInteract;
            playerInput.actions["Player/Interact"].canceled += OnInteract;
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
            {
                return;
            }

            Debug.Log((int)interactableLayer);
            FindClosestInteractable();
        }

        private void FindClosestInteractable()
        {
            int closestInteractableIndex;
            float closestInteractableSqrDistance;

            

            float GetSqrDistance(int i)
            {
                Vector3 closestPoint = interactablesInRange[i].ClosestPoint(trigger.center);
                float sqrDistance = GeneralUtils.SqrDistance(closestPoint, trigger.center);
                return sqrDistance;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == interactableLayer)
            {
                interactablesInRange.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != interactableLayer)
            {
                return;
            }
            interactablesInRange.Remove(other);
        }
    }
}