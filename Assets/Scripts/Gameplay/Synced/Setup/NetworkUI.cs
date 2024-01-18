using GrassGame.Gameplay.Local;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace GrassGame.Gameplay.Synced
{
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
        [SerializeField] private Button playerButton;
        [SerializeField] private Button scanViewButton;

        private void Awake()
        {
            networkButtonsParent.SetActive(true);

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

            playerButton.onClick.AddListener(() =>
            {
                LocalGameManager.Instance.StartPlayer();
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
        }
    }
}