using TMPro;
using UnityEngine;

namespace GrassGame.Gameplay.Local.Dialogue.Readers
{
    public class TitleReader : DialogueReader
    {
        [SerializeField] private TextMeshProUGUI titleTextUGUI;
        public override void OnNextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args)
        {
            titleTextUGUI.text = args.Title;
        }
    }
}