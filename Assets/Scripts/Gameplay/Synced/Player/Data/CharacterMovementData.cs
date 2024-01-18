using Unity.Netcode;
using UnityEngine;

namespace GrassGame.Gameplay.Synced.Player.Data
{
    public struct CharacterMovementData : INetworkSerializable
    {
        #region Private
        private Vector3 velocity;
        private Vector3 moveDirection;
        private Vector3 position;
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

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public static CharacterMovementData Zero
        {
            get
            {
                return new CharacterMovementData(Vector3.zero, Vector3.zero, Vector3.zero);
            }
        }
        #endregion

        public CharacterMovementData(Vector3 velocity, Vector3 moveDirection, Vector3 position)
        {
            this.velocity = velocity;
            this.moveDirection = moveDirection;
            this.position = position;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref velocity);
            serializer.SerializeValue(ref moveDirection);
            serializer.SerializeValue(ref position);
        }
    }
}