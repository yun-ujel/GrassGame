using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrassGame.Gameplay.Local.Dialogue.Readers
{
    public abstract class DialogueReader : MonoBehaviour
    {
        public virtual void Start()
        {
            DialogueManager.Instance.OnNextDialogueEvent += OnNextDialogue;
        }

        public abstract void OnNextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args);
    }
}