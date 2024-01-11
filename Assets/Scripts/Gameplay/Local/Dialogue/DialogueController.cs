using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI;
using TMPro;

using GrassGame.Utilities;
using DS.ScriptableObjects;
using DS.Enumerations;

namespace GrassGame.Gameplay.Local.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private DSDialogueSO currentDialogueSO;
        private bool isDialogueOpen;

        private PlayerInput playerInput;
        [SerializeField] private Animator animator;
        
        [Space]
        
        [SerializeField] private TextMeshProUGUI quoteUGUI;
        [SerializeField, Range(1f, 100f)] private float lettersPerSecond;
        private float timeSinceLetterAdded;

        private char[] targetQuoteCharArray;
        private string targetQuote;
        private string currentQuote;
        private int quoteProgress;

        private bool IsQuoteFinished => targetQuoteCharArray != null && quoteProgress == targetQuoteCharArray.Length && currentQuote == targetQuote;

        [Space]

        [SerializeField] private DialogueOptionController optionController;

        private void Start()
        {
            playerInput = GeneralUtils.GetPlayerInput();
            playerInput.actions["UI/Submit"].performed += OnSubmit;

            DialogueManager.Instance.OnStartDialogueEvent += StartDialogue;
            DialogueManager.Instance.OnNextDialogueEvent += NextDialogue;
            DialogueManager.Instance.OnExitDialogueEvent += ExitDialogue;

            optionController.OnOptionSelectEvent += OnOptionSelected;
        }

        private void Update()
        {
            if (!IsQuoteFinished && isDialogueOpen)
            {
                UpdateCurrentQuote(Time.deltaTime);
            }
        }

        private void OnOptionSelected(object sender, DialogueOptionController.OnOptionSelectEventArgs args)
        {

        }

        #region Input Events
        private void OnSubmit(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
            {
                return;
            }

            if (IsQuoteFinished && currentDialogueSO.DialogueType == DSDialogueType.SingleChoice)
            {
                DialogueManager.Instance.NextDialogue();
                return;
            }

            FinishCurrentQuote();
        }
        #endregion

        #region Dialogue Events
        private void StartDialogue(object sender, DialogueManager.OnStartDialogueEventArgs args)
        {
            animator.Play("Open");
            isDialogueOpen = true;
        }

        private void NextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args)
        {
            // Display Next Dialogue
            SetTargetQuote(args.Text);
            currentDialogueSO = args.NextDialogueSO;
        }

        private void ExitDialogue(object sender, DialogueManager.OnExitDialogueEventArgs args)
        {
            animator.Play("Close");
            isDialogueOpen = false;
        }
        #endregion

        #region Quote Methods
        private void SetTargetQuote(string quote)
        {
            targetQuoteCharArray = quote.ToCharArray();
            targetQuote = quote;

            currentQuote = string.Empty + targetQuoteCharArray[0];
            quoteProgress = 1;
        }

        private void UpdateCurrentQuote(float delta)
        {
            float rate = 1f / lettersPerSecond;
            int lettersToAdd = Mathf.FloorToInt(timeSinceLetterAdded / rate);

            if (lettersToAdd > 0)
            {
                for (int i = 0; i < lettersToAdd; i++)
                {
                    if (quoteProgress >= targetQuoteCharArray.Length)
                    {
                        FinishCurrentQuote();
                        break;
                    }

                    currentQuote += targetQuoteCharArray[quoteProgress];
                    quoteProgress++;
                }
                timeSinceLetterAdded = timeSinceLetterAdded % rate;
            }

            timeSinceLetterAdded += delta;

            quoteUGUI.text = currentQuote;
        }

        private void FinishCurrentQuote()
        {
            currentQuote = targetQuote;
            quoteProgress = targetQuoteCharArray.Length;

            quoteUGUI.text = currentQuote;
            optionController.LoadOptions(currentDialogueSO);
        }
        #endregion
    }
}