using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    #region Parameters
    struct PlayerMovementData : INetworkSerializable
    {
        #region Private
        private Vector3 velocity;
        private Vector3 moveDirection;
        #endregion

        #region Public
        public Vector3 MoveDirection
        {
            get { return moveDirection; }
            set { moveDirection = value; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        #endregion

        public PlayerMovementData(Vector3 velocity, Vector3 moveDirection)
        {
            this.velocity = velocity;
            this.moveDirection = moveDirection;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref velocity);
            serializer.SerializeValue(ref moveDirection);
        }
    }

    private NetworkVariable<PlayerMovementData> networkMovementData = new(new PlayerMovementData(Vector3.zero, Vector3.zero), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> networkPosition = new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private PlayerMovementData movementData;

    private Rigidbody body;

    private PlayerInput playerInput;
    private readonly string playerInputTag = "PlayerInput";

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

    #endregion

    #region Initialisation
    public override void OnNetworkSpawn()
    {
        body = GetComponent<Rigidbody>();

        if (IsOwner)
        {
            playerInput = GameObject.FindGameObjectWithTag(playerInputTag).GetComponent<PlayerInput>();

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
        playerInput.actions["Move"].performed += OnPerformMove;
        playerInput.actions["Move"].canceled += OnCancelMove;
    }

    private void UnsubscribeFromInputActions()
    {
        playerInput.actions["Move"].performed -= OnPerformMove;
        playerInput.actions["Move"].canceled -= OnCancelMove;
    }
    #endregion

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            ProcessMovement();
        }
        else
        {
            ReadMovement();
        }
    }

    private void ReadMovement()
    {
        body.velocity = networkMovementData.Value.Velocity;
        Debug.Log(body.velocity);
    }

    private void ProcessMovement()
    {
        body.velocity = Vector3.MoveTowards(body.velocity, movementData.MoveDirection * maxSpeed, acceleration * Time.fixedDeltaTime);
        movementData.Velocity = body.velocity;

        networkMovementData.Value = movementData;
    }

    #region Input Events

    private void OnPerformMove(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        Debug.Log(movement);

        movementData.MoveDirection = new Vector3(movement.x, 0f, movement.y);

        networkMovementData.Value = movementData;
    }

    private void OnCancelMove(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        Debug.Log(movement);

        movementData.MoveDirection = new Vector3(movement.x, 0f, movement.y);

        networkMovementData.Value = movementData;
    }

    #endregion
}
