using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using GrassGame.Gameplay.Synced.Player.Enumerations;
using GrassGame.Gameplay.Local;
using GrassGame.Utilities;

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
        [SerializeField] private GameObject characterSelectParent;

        [Space]

        [SerializeField] private Button archibaldButton;
        [SerializeField] private Button jacksonButton;
        [SerializeField] private Button sheilaButton;

        private void Awake()
        {
            networkButtonsParent.SetActive(true);
            localButtonsParent.SetActive(false);
            characterSelectParent.SetActive(false);

            GetComponent<Graphic>().enabled = true;

            SubscribeToAllButtons();
        }

        private void SubscribeToAllButtons()
        {
            
            hostButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartHost();
                OpenLocalButtons();
            });
            serverButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartServer();
                OpenLocalButtons();
            });
            clientButton.onClick.AddListener(() =>
            {
                _ = NetworkManager.Singleton.StartClient();
                OpenLocalButtons();
            });
            
            characterButton.onClick.AddListener(() =>
            {
                OpenCharacterSelect();
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

        private void OpenCharacterSelect()
        {
            networkButtonsParent.SetActive(false);
            localButtonsParent.SetActive(false);
            characterSelectParent.SetActive(true);
        }
        private void OpenLocalButtons()
        {
            networkButtonsParent.SetActive(false);
            localButtonsParent.SetActive(true);
        }

        private void CloseAllButtons()
        {
            networkButtonsParent.SetActive(false);
            localButtonsParent.SetActive(false);
            characterSelectParent.SetActive(false);
            GetComponent<Graphic>().enabled = false;
            gameObject.SetActive(false);
        }

        private void StartCharacter(CharacterType characterType)
        {
            LocalGameManager.Instance.StartCharacter(characterType);
            CloseAllButtons();
        }
    }
}