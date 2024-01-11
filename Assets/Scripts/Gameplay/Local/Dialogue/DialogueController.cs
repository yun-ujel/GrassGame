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
        private string currentQuote;
        private int quoteProgress;

        private bool IsQuoteFinished => targetQuoteCharArray != null && quoteProgress == targetQuoteCharArray.Length && targetQuoteCharArray.Length > 0;

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

            if (!IsQuoteFinished)
            {
                FinishCurrentQuote();
            }
        }

        private void OnOptionSelected(object sender, DialogueOptionController.OnOptionSelectEventArgs args)
        {
            DialogueManager.Instance.NextDialogue(args.SelectedOptionIndex);
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

            currentQuote = string.Empty + targetQuoteCharArray[0];
            quoteProgress = 1;

            timeSinceLetterAdded = 0;
        }

        private void UpdateCurrentQuote(float delta)
        {
            float rate = 1f / lettersPerSecond;
            int lettersToAdd = Mathf.FloorToInt(timeSinceLetterAdded / rate);

            if (lettersToAdd > 0)
            {
                for (int i = 0; i < lettersToAdd; i++)
                {
                    currentQuote += targetQuoteCharArray[quoteProgress];
                    quoteProgress++;

                    if (IsQuoteFinished)
                    {
                        FinishCurrentQuote();
                        break;
                    }
                }
                timeSinceLetterAdded = timeSinceLetterAdded % rate;
            }

            timeSinceLetterAdded += delta;

            quoteUGUI.text = currentQuote;
        }

        private void FinishCurrentQuote()
        {
            currentQuote = currentDialogueSO.Text;
            quoteProgress = targetQuoteCharArray.Length;

            quoteUGUI.text = currentQuote;
            
            if (currentDialogueSO.DialogueType == DSDialogueType.MultipleChoice)
            {
                optionController.LoadOptions(currentDialogueSO);
            }
        }
        #endregion
    }
}