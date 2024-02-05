using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassGame.Gameplay.Local
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventSender : MonoBehaviour
    {
        public class EventArgs : System.EventArgs
        {
            public Collider Other { get; private set; }
            public int Index { get; private set; }

            public EventArgs(Collider other, int index)
            {
                Other = other;
                Index = index;
            }
        }

        public event System.EventHandler<EventArgs> OnTriggerEnterEvent;
        public event System.EventHandler<EventArgs> OnTriggerExitEvent;

        public Collider Trigger { get; private set; }
        public int Index { get; set; } = -1;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(this, new EventArgs(other, Index));
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(this, new EventArgs(other, Index));
        }

        private void Start()
        {
            Trigger = GetComponent<Collider>();
            if (!Trigger.isTrigger)
            {
                Debug.LogError($"TriggerEventSender Trigger {Trigger} is set to act as a Collider");
            }
        }
    }
}