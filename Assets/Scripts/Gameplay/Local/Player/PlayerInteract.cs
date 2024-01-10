using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using GrassGame.Utilities;
using GrassGame.Gameplay.Local.Interactables;

namespace GrassGame.Gameplay.Local.Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerInteract : MonoBehaviour
    {
        private PlayerInput playerInput;

        [Space]

        [SerializeField] private int interactableLayer;
        [SerializeField] private SphereCollider trigger;

        private List<Collider> interactablesInRange;

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

            FindClosestInteractable()?.TriggerInteract();
        }

        private Interactable FindClosestInteractable()
        {
            if (interactablesInRange.Count <= 0)
            {
                return null;
            }

            int closestInteractableIndex = 0;
            float closestInteractableSqrDistance = GetSqrDistanceOfInteractable(0);

            for (int i = 0; i < interactablesInRange.Count; i++)
            {
                if (GetSqrDistanceOfInteractable(i) > closestInteractableSqrDistance)
                {
                    closestInteractableIndex = i;
                }
            }

            return interactablesInRange[closestInteractableIndex].GetComponent<Interactable>();

            float GetSqrDistanceOfInteractable(int i)
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