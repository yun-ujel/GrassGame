using GrassGame.Gameplay.Local;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private Button spectatorButton;
        [SerializeField] private Button characterButton;
        [SerializeField] private Button scanViewButton;

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
                LocalGameManager.Instance.StartCharacter();
                CloseButtons();
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
    }
}