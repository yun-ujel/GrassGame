using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

using GrassGame.Utilities;
using GrassGame.Gameplay.Synced.Player.Data;

namespace GrassGame.Gameplay.Synced.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : NetworkBehaviour
    {
        #region Parameters

        private NetworkVariable<CharacterMovementData> networkMovementData = new(CharacterMovementData.Zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        private CharacterMovementData movementData;

        private Rigidbody body;

        private PlayerInput playerInput;

        [Header("Write: Speed Settings")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;

        [Header("Write: Camera Settings")]
        [SerializeField] private bool moveRelativeToCamera;
        [SerializeField] private Camera mainCamera;

        [Header("Read: Interpolation Settings")]
        [SerializeField, Range(-1, 1)] private float minSnapDot = 0.2f;
        [SerializeField] private float minSnapDistance = 1f;

        #endregion

        #region Initialisation
        public override void OnNetworkSpawn()
        {
            body = GetComponent<Rigidbody>();

            if (IsOwner)
            {
                playerInput = GeneralUtils.GetPlayerInput();

                SubscribeToInputActions();
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                UnsubscribeFromInputActions();
            }
        }

        private void SubscribeToInputActions()
        {
            playerInput.actions["Player/Move"].performed += OnPerformMove;
            playerInput.actions["Player/Move"].canceled += OnPerformMove;
        }

        private void UnsubscribeFromInputActions()
        {
            playerInput.actions["Player/Move"].performed -= OnPerformMove;
            playerInput.actions["Player/Move"].canceled -= OnPerformMove;
        }
        #endregion

        private void FixedUpdate()
        {
            if (IsOwner)
            {
                TransmitMovement();
            }
            else
            {
                ReadMovement();
            }
        }

        private void TransmitMovement()
        {
            Vector3 velocity = Vector3.MoveTowards(body.velocity, movementData.MoveDirection * maxSpeed, acceleration * Time.fixedDeltaTime);
            velocity.y = body.velocity.y;

            body.velocity = velocity;

            movementData.Velocity = body.velocity;
            movementData.Position = body.position;

            networkMovementData.Value = movementData;
        }

        #region Read and Interpolate State

        private Vector3 interpolateVelocity;

        private void ReadMovement()
        {
            // Interpolating between network positions is done in two ways:
            // The Rigidbody's velocity is synced across clients (this is smooth, but delays in velocity changes will result in desynced positions)
            // The position is smoothly re-synced under specific conditions ("Snap Position")

            if (CanSnapPosition(body.position, networkMovementData.Value.Position, body.velocity))
            {
                // "Snap", or smoothly move towards the synced position without relation to input
                body.MovePosition(Vector3.SmoothDamp(body.position, networkMovementData.Value.Position, ref interpolateVelocity, Time.fixedDeltaTime * 2f));
            }

            body.velocity = networkMovementData.Value.Velocity;
        }

        private bool CanSnapPosition(Vector3 currentPosition, Vector3 snapPosition, Vector3 currentMoveDirection)
        {
            Vector3 snapDirection = snapPosition - currentPosition;
            float sqrDistance = snapDirection.sqrMagnitude;

            // If the positions are too far desynced, or resyncing the position wouldn't cause jittery movement (dot product), return true
            return currentMoveDirection.sqrMagnitude == 0 || (Vector3.Dot(snapDirection, currentMoveDirection) >= minSnapDot && sqrDistance >= Mathf.Pow(minSnapDistance, 2));
        }

        #endregion

        #region Input Events

        private void OnPerformMove(InputAction.CallbackContext ctx)
        {
            Vector2 movement = ctx.ReadValue<Vector2>();

            if (moveRelativeToCamera)
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }

                Vector3 moveDirection = movement.y * mainCamera.transform.forward; // Forward Movement
                moveDirection += movement.x * mainCamera.transform.right; // Horizontal Movement
                moveDirection.y = 0f;

                moveDirection.Normalize();

                movementData.MoveDirection = moveDirection;
            }
            else
            {
                movementData.MoveDirection = new Vector3(movement.x, 0f, movement.y);
            }

            networkMovementData.Value = movementData;
        }

        #endregion
    }

}