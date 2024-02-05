using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassGame.Gameplay.Local
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Follow Target")]
        [SerializeField] private Transform target;
        [SerializeField] private float distanceFromTarget;

        [Space]
        [Space]

        [SerializeField] private Vector3 rotation;
        [SerializeField] private Vector3 globalOffset;

        private Vector3 targetPosition;
        private Vector3 targetLookDirection;

        [Header("Bounds")]
        [SerializeField] private TriggerEventSender[] triggers;
        [SerializeField, Tooltip("If false, this script will only follow the target if it's inside a trigger.")]
        private bool followOutsideTriggers;

        private bool[] enteredTriggers;

        private void Start()
        {
            FollowTarget();
            SubscribeToTriggers();
        }

        private void Update()
        {
            FollowTarget();
        }

        #region Follow Target Methods
        private void FollowTarget()
        {
            if (target == null)
            {
                return;
            }

            if (!IsTargetInBounds(out int index))
            {
                if (index != -1)
                {
                    Vector3 closestPoint = triggers[index].Trigger.ClosestPoint(target.position);
                    Debug.DrawRay(closestPoint, Vector3.up, Color.red);
                    Orbit(closestPoint);
                }

                return;
            }
            Orbit(target.position);
        }

        private void Orbit(Vector3 targetPoint)
        {
            targetLookDirection = Quaternion.Euler(rotation) * Vector3.forward;
            targetPosition = (targetLookDirection * -distanceFromTarget) + targetPoint + globalOffset;

            transform.position = targetPosition;
            transform.forward = targetLookDirection;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        #endregion

        #region Bounds Methods
        private void SubscribeToTriggers()
        {
            enteredTriggers = new bool[triggers.Length];

            for (int i = 0; i < triggers.Length; i++)
            {
                triggers[i].Index = i;

                triggers[i].OnTriggerEnterEvent += OnTriggerEntered;
                triggers[i].OnTriggerExitEvent += OnTriggerExited;
            }
        }

        private void OnTriggerEntered(object sender, TriggerEventSender.EventArgs args)
        {
            enteredTriggers[args.Index] = true;
        }

        private void OnTriggerExited(object sender, TriggerEventSender.EventArgs args)
        {
            enteredTriggers[args.Index] = false;
        }

        private bool IsTargetInBounds(out int insideTriggerIndex)
        {
            insideTriggerIndex = -1;
            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i])
                {
                    insideTriggerIndex = i;
                    break;
                }
            }

            return insideTriggerIndex == -1 ? followOutsideTriggers : !followOutsideTriggers;
        }
        #endregion
    }
}