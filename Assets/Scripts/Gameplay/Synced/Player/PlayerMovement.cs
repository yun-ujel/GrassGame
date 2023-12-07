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

    [Header("Write: Speed Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;

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
        playerInput.actions["Move"].canceled += OnPerformMove;
    }

    private void UnsubscribeFromInputActions()
    {
        playerInput.actions["Move"].performed -= OnPerformMove;
        playerInput.actions["Move"].canceled -= OnPerformMove;
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

        networkPosition.Value = body.position;
        networkMovementData.Value = movementData;
    }

    #region Read and Interpolate State

    private Vector3 interpolateVelocity;

    private void ReadMovement()
    {
        if (CanSnapPosition(body.position, networkPosition.Value, body.velocity))
        {
            body.MovePosition(Vector3.SmoothDamp(body.position, networkPosition.Value, ref interpolateVelocity, Time.fixedDeltaTime * 2f));
        }

        body.velocity = networkMovementData.Value.Velocity;
    }

    private bool CanSnapPosition(Vector3 currentPosition, Vector3 snapPosition, Vector3 currentMoveDirection)
    {
        Vector3 snapDirection = snapPosition - currentPosition;
        float sqrDistance = snapDirection.sqrMagnitude;

        return currentMoveDirection.sqrMagnitude == 0 || (Vector3.Dot(snapDirection, currentMoveDirection) >= minSnapDot && sqrDistance >= Mathf.Pow(minSnapDistance, 2));
    }

    #endregion

    #region Input Events

    private void OnPerformMove(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();
        Debug.Log($"{OwnerClientId}; {movement}");

        movementData.MoveDirection = new Vector3(movement.x, 0f, movement.y);

        networkMovementData.Value = movementData;
    }

    #endregion
}
