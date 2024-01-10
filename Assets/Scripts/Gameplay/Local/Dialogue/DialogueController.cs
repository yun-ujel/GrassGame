using UnityEngine;
using UnityEngine.InputSystem;

using GrassGame.Utilities;

namespace GrassGame.Gameplay.Local.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private PlayerInput playerInput;
        [SerializeField] private Animator animator;
        private void Start()
        {
            playerInput = GeneralUtils.GetPlayerInput();
            playerInput.actions["UI/Submit"].performed += OnSubmit;

            DialogueManager.Instance.OnStartDialogueEvent += StartDialogue;
            DialogueManager.Instance.OnNextDialogueEvent += NextDialogue;
            DialogueManager.Instance.OnExitDialogueEvent += ExitDialogue;
        }

        private void OnSubmit(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
            {
                return;
            }

            DialogueManager.Instance.NextDialogue();
        }

        private void StartDialogue(object sender, DialogueManager.OnStartDialogueEventArgs args)
        {
            animator.Play("Open");
        }

        private void NextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args)
        {
            // Display Next Dialogue
        }

        private void ExitDialogue(object sender, DialogueManager.OnExitDialogueEventArgs args)
        {
            animator.Play("Close");
        }
    }
}