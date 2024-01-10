using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GrassGame.Gameplay.Local.Dialogue.Readers
{
    public class PortraitReader : DialogueReader
    {
        [SerializeField] private GameObject portraitRoot;
        [SerializeField] private RawImage portraitDisplay;
        public override void OnNextDialogue(object sender, DialogueManager.OnNextDialogueEventArgs args)
        {
            if (args.NextDialogueSO == null || args.NextDialogueSO.Texture == null)
            {
                portraitRoot.SetActive(false);
                return;
            }
            portraitRoot.SetActive(true);
            portraitDisplay.texture = args.NextDialogueSO.Texture;
        }
    }
}