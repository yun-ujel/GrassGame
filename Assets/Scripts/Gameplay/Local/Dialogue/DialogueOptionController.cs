using UnityEngine;

using TMPro;
using UnityEngine.UI;

using DS.ScriptableObjects;

namespace GrassGame.Gameplay.Local.Dialogue
{
    public class DialogueOptionController : MonoBehaviour
    {
        public class OnOptionSelectEventArgs : System.EventArgs
        {
            public int SelectedOptionIndex { get; private set; }
            public OnOptionSelectEventArgs(int selectedOptionIndex)
            {
                SelectedOptionIndex = selectedOptionIndex;
            }
        }
        public event System.EventHandler<OnOptionSelectEventArgs> OnOptionSelectEvent;

        [SerializeField] private Transform OptionsContainer;
        [Space]
        [SerializeField] private GameObject OptionPrefab;
        public void LoadOptions(DSDialogueSO dialogue)
        {
            string[] choices = dialogue.GetChoicesAsStringArray();
            
            for (int i = 0; i < choices.Length; i++)
            {
                CreateOption(i, choices[i]);
            }
        }

        private void CreateOption(int index, string text)
        {
            GameObject clone = Instantiate(OptionPrefab, OptionsContainer);
            clone.name = text;

            DialogueOption option = clone.GetComponent<DialogueOption>();
            option.Index = index;
            option.SetText(text);

            option.OnClickEvent += OnButtonClick;
        }

        private void OnButtonClick(object sender, DialogueOption.OnClickEventArgs args)
        {
            DeleteContainerChildren();
            OnOptionSelectEvent?.Invoke(this, new OnOptionSelectEventArgs(args.Index));
        }

        private void DeleteContainerChildren()
        {
            for (int i = 0; i < OptionsContainer.childCount; i++)
            {
                Destroy(OptionsContainer.GetChild(i).gameObject);
            }
        }
    }
}