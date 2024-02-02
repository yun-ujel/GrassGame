using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassGame.Gameplay.Local
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 rotation;
        [SerializeField] private float distanceFromTarget;
        [SerializeField] private Vector3 offset;

        [Space]

        [SerializeField] private Transform target;
        
        private Vector3 targetPosition;
        private Vector3 targetLookDirection;

        private void Start()
        {
            transform.forward = Vector3.forward;
            FollowTarget();
        }

        private void Update()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (target == null)
            {
                return;
            }

            targetLookDirection = Quaternion.Euler(rotation) * Vector3.forward;
            targetPosition = (targetLookDirection * -distanceFromTarget) + target.position + offset;

            transform.position = targetPosition;
            transform.forward = targetLookDirection;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}