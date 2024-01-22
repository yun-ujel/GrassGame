using UnityEngine;
using UnityEngine.InputSystem;

using GrassGame.Utilities;

namespace GrassGame.Gameplay.Local.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        #region Parameters

        #region Private
        private Rigidbody body;
        private PlayerInput playerInput;

        [Header("Speed Settings")]
        [SerializeField] private float maxSpeed = 8f;
        [SerializeField] private float acceleration = 80f;

        [Header("Camera Settings")]
        [SerializeField] private bool moveRelativeToCamera = true;
        [SerializeField] private Camera mainCamera;

        private Vector3 velocity;
        private Vector3 moveDirection;
        #endregion

        #region Public
        public Vector3 MoveDirection
        {
            get { return moveDirection; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
        }

        public Vector3 Position
        {
            get { return transform.position; }
        }
        #endregion
        #endregion

        private void Start()
        {
            body = GetComponent<Rigidbody>();
            playerInput = GeneralUtils.GetPlayerInput();
            SubscribeToInputActions();
        }

        private void SubscribeToInputActions()
        {
            playerInput.actions["Player/Move"].performed += OnPerformMove;
            playerInput.actions["Player/Move"].canceled += OnPerformMove;
        }

        private void FixedUpdate()
        {
            velocity = Vector3.MoveTowards(body.velocity, moveDirection * maxSpeed, acceleration * Time.fixedDeltaTime);
            velocity.y = body.velocity.y;

            body.velocity = velocity;
        }

        private void OnPerformMove(InputAction.CallbackContext ctx)
        {
            Vector2 movement = ctx.ReadValue<Vector2>();

            if (moveRelativeToCamera)
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }

                Vector3 direction = GeneralUtils.RemapDirection(movement, mainCamera.transform);

                direction.Normalize();

                moveDirection = direction;
                return;
            }

            moveDirection = new Vector3(movement.x, 0f, movement.y);
        }
    }
}