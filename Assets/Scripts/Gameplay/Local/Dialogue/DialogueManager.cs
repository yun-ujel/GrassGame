using UnityEngine;
using UnityEngine.InputSystem;

using GrassGame.Utilities;
using DS.ScriptableObjects;

namespace GrassGame.Gameplay.Local.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        private PlayerInput playerInput;

        #region Event Args
        public class OnStartDialogueEventArgs : System.EventArgs
        {
            public OnStartDialogueEventArgs()
            {

            }
        }
        public class OnNextDialogueEventArgs : System.EventArgs
        {
            public OnNextDialogueEventArgs(DSDialogueSO next)
            {
                NextDialogueSO = next;
                if (next != null)
                {
                    Text = next.Text;
                    Title = next.Title;
                }
            }

            public DSDialogueSO NextDialogueSO { get; private set; }
            public string Title { get; private set; }
            public string Text { get; private set; }
        }
        public class OnExitDialogueEventArgs : System.EventArgs
        {
            public OnExitDialogueEventArgs()
            {

            }
        }
        #endregion

        private DSDialogueSO currentDialogue;

        public event System.EventHandler<OnStartDialogueEventArgs> OnStartDialogueEvent;
        public event System.EventHandler<OnNextDialogueEventArgs> OnNextDialogueEvent;
        public event System.EventHandler<OnExitDialogueEventArgs> OnExitDialogueEvent;

        private void SetupInstance()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        private void Awake()
        {
            SetupInstance();

            playerInput = GeneralUtils.GetPlayerInput();
        }

        #region Dialogue Advancing Methods
        public void StartDialogue(DSDialogueSO dialogue)
        {
            playerInput.SwitchCurrentActionMap("UI");

            OnStartDialogueEvent?.Invoke(this, new OnStartDialogueEventArgs());
            OnNextDialogueEvent?.Invoke(this, new OnNextDialogueEventArgs(dialogue));

            currentDialogue = dialogue;
        }

        public void NextDialogue(int choice = 0)
        {
            DSDialogueSO chosenDialogue = currentDialogue.GetChoice(0);

            if (chosenDialogue == null)
            {
                ExitDialogue();
                return;
            }

            OnNextDialogueEvent?.Invoke(this, new OnNextDialogueEventArgs(chosenDialogue));
            currentDialogue = chosenDialogue;
        }

        public void ExitDialogue()
        {
            OnExitDialogueEvent?.Invoke(this, new OnExitDialogueEventArgs());

            playerInput.SwitchCurrentActionMap("Player");
        }
        #endregion
    }
}