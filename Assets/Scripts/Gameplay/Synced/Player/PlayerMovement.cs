using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    struct PlayerNetworkData : INetworkSerializable
    {
        private float xPos;
        private float yPos;
        private float zPos;

        public Vector3 Position
        {
            get => new Vector3 (xPos, yPos, zPos);
            set
            {
                xPos = value.x;
                yPos = value.y;
                zPos = value.z;
            }
        }

        public PlayerNetworkData(float x, float y, float z)
        {
            xPos = x;
            yPos = y;
            zPos = z;
        }

        public PlayerNetworkData(Vector3 pos)
        {
            xPos = pos.x;
            yPos = pos.y;
            zPos = pos.z;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref xPos);
            serializer.SerializeValue(ref yPos);
            serializer.SerializeValue(ref zPos);
        }
    }

    private NetworkVariable<PlayerNetworkData> networkData = new NetworkVariable<PlayerNetworkData>(new PlayerNetworkData(Vector3.zero), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private PlayerInput playerInput;
    private readonly string playerInputTag = "PlayerInput";

    private void Update()
    {
        transform.position = networkData.Value.Position;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        playerInput = GameObject.FindGameObjectWithTag(playerInputTag).GetComponent<PlayerInput>();
        
        SubscribeToInputActions();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        UnsubscribeFromInputActions();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 movement = ctx.ReadValue<Vector2>();

        networkData.Value = new PlayerNetworkData(networkData.Value.Position + new Vector3(movement.x, 0f, movement.y));
    }

    private void SubscribeToInputActions()
    {
        playerInput.actions["Move"].performed += OnMove;
    }

    private void UnsubscribeFromInputActions()
    {
        playerInput.actions["Move"].performed -= OnMove;
    }
}
