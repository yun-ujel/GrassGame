using TMPro;
using UnityEngine;

namespace GrassGame.Gameplay.Local.Dialogue.Readers
{
    public class TextReader : DialogueReader
    {
        [SerializeField] private TextMeshProUGUI quoteTextUGUI;
        public override void OnNextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args)
        {
            quoteTextUGUI.text = args.Text;
        }
    }
}