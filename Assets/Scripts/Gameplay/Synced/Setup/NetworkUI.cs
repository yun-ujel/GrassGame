using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

using GrassGame.Gameplay.Synced.Player.Enumerations;
using GrassGame.Gameplay.Local;

namespace GrassGame.Gameplay.Synced
{
    [RequireComponent(typeof(Graphic))]
    public class NetworkUI : MonoBehaviour
    {
        [Header("Network Buttons")]
        [SerializeField] private GameObject networkButtonsParent;

        [Space]

        [SerializeField] private Button hostButton;
        [SerializeField] private Button serverButton;
        [SerializeField] private Button clientButton;

        [Header("Local Buttons")]
        [SerializeField] private GameObject localButtonsParent;

        [Space]

        [SerializeField] private Button spectatorButton;
        [SerializeField] private Button characterButton;
        [SerializeField] private Button scanViewButton;

        [Header("Character Buttons")]
        [SerializeField] private GameObject characterButtonsParent;

        [Space]

        [SerializeField] private Button archibaldButton;
        [SerializeField] private Button jacksonButton;
        [SerializeField] private Button sheilaButton;

        private void Awake()
        {
            networkButtonsParent.SetActive(true);
            localButtonsParent.SetActive(false);
            GetComponent<Graphic>().enabled = true;

            hostButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartHost();
                SwitchToLocalButtons();
            });
            serverButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartServer();
                SwitchToLocalButtons();
            });
            clientButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartClient();
                SwitchToLocalButtons();
            });

            characterButton.onClick.AddListener(() =>
            {
                CloseButtons();
            });

            archibaldButton.onClick.AddListener(() =>
            {
                StartCharacter(CharacterType.Archibald);
            });
            jacksonButton.onClick.AddListener(() =>
            {
                StartCharacter(CharacterType.Jackson);
            });
            sheilaButton.onClick.AddListener(() =>
            {
                StartCharacter(CharacterType.Sheila);
            });
        }

        private void SwitchToLocalButtons()
        {
            networkButtonsParent.SetActive(false);
            localButtonsParent.SetActive(true);
        }

        private void CloseButtons()
        {
            networkButtonsParent.SetActive(false);
            localButtonsParent.SetActive(false);
            gameObject.SetActive(false);
        }

        private void StartCharacter(CharacterType characterType)
        {
            LocalGameManager.Instance.StartCharacter(characterType);
        }
    }
}