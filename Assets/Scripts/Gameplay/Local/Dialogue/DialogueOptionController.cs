using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DS.ScriptableObjects;
using GrassGame.Utilities;

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

            if (choices == null || choices.Length == 0)
            {
                return;
            }

            GameObject option = null;

            for (int i = 0; i < choices.Length; i++)
            {
                if (i == 0)
                {
                    option = CreateOption(i, choices[i]);
                    continue;
                }
                CreateOption(i, choices[i]);
            }

            EventSystem.current.UIFocus(option);
        }

        private GameObject CreateOption(int index, string text)
        {
            GameObject clone = Instantiate(OptionPrefab, OptionsContainer);
            clone.name = text;

            DialogueOption option = clone.GetComponent<DialogueOption>();
            option.Index = index;
            option.SetText(text);

            option.OnClickEvent += OnButtonClick;

            return clone;
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