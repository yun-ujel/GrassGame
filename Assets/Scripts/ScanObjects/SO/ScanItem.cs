using UnityEngine;

namespace GrassGame.ScanObjects.SO
{
    public class ScanItem : ScriptableObject
    {
        public class OnRemovedEventArgs : System.EventArgs
        {
            public int Index { get; private set; }

            public OnRemovedEventArgs(int index)
            {
                Index = index;
            }
        }

        public event System.EventHandler<OnRemovedEventArgs> OnRemovedEvent;
        [field: SerializeField, HideInInspector] public ScanItemData Data { get; set; }
        public void Remove()
        {
            OnRemovedEvent?.Invoke(this, new OnRemovedEventArgs(Data.Index));
        }
    }
}