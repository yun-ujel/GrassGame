using UnityEngine;

using GrassGame.Gameplay.Synced.Player.Data;

namespace GrassGame.Gameplay.Synced.Player.Readers
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovementReader : MonoBehaviour
    {
        private Rigidbody body;
        private Vector3 interpolateVelocity;

        [Header("Interpolation Settings")]
        [SerializeField, Range(-1, 1)] private float minSnapDot = 0.2f;
        [SerializeField] private float minSnapDistance = 1f;

        public void OnMovementDataChanged(CharacterMovementData prevData, CharacterMovementData currentData)
        {
            ReadMovement(currentData);
        }

        private void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        private void ReadMovement(CharacterMovementData movementData)
        {
            // Interpolating between network positions is done in two ways:
            // The Rigidbody's velocity is synced across clients (this is smooth, but delays in velocity changes will result in desynced positions)
            // The position is smoothly re-synced under specific conditions ("Snap Position")

            if (CanSnapPosition(body.position, movementData.Position, body.velocity))
            {
                // "Snap", or smoothly move towards the synced position without relation to input
                body.MovePosition(Vector3.SmoothDamp(body.position, movementData.Position, ref interpolateVelocity, Time.fixedDeltaTime * 2f));
            }

            body.velocity = movementData.Velocity;
        }

        private bool CanSnapPosition(Vector3 currentPosition, Vector3 snapPosition, Vector3 currentMoveDirection)
        {
            Vector3 snapDirection = snapPosition - currentPosition;
            float sqrDistance = snapDirection.sqrMagnitude;

            // If the positions are too far desynced, or resyncing the position wouldn't cause jittery movement (dot product), return true
            return currentMoveDirection.sqrMagnitude == 0 || (Vector3.Dot(snapDirection, currentMoveDirection) >= minSnapDot && sqrDistance >= Mathf.Pow(minSnapDistance, 2));
        }
    }
}