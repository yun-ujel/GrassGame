using UnityEngine;

using GrassGame.Gameplay.Local.Dialogue;
using DS;

namespace GrassGame.Gameplay.Local.Interactables
{
    [RequireComponent(typeof(DSDialogue))]
    public class DialogueInteractable : Interactable
    {
        DSDialogue storedDialogue;
        private void Start()
        {
            storedDialogue = gameObject.GetComponent<DSDialogue>();
        }

        public override void TriggerInteract()
        {
            DialogueManager.Instance.StartDialogue(storedDialogue.dialogue);
        }
    }
}