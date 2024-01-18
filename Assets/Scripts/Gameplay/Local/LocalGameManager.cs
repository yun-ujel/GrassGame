using GrassGame.Gameplay.Synced;
using UnityEngine;

namespace GrassGame.Gameplay.Local
{
    public class LocalGameManager : MonoBehaviour
    {
        public static LocalGameManager Instance { get; private set; }
        public static PlayerSync LocalPlayer { get; private set; }

        [Header("Prefabs: Writers")]
        [SerializeField] private GameObject characterWriterPrefab;

        private void Awake()
        {
            SetupInstance();
        }

        private void SetupInstance()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        public void SetLocalPlayer(PlayerSync player)
        {
            Debug.Log($"Local Player being set to {player}");
            LocalPlayer = player;
        }

        private void LoadOtherCharacters()
        {

        }

        #region Start Methods
        public void StartCharacter()
        {
            GameObject character = Instantiate(characterWriterPrefab);
            LocalPlayer.StartCharacter(character.GetComponent<Rigidbody>());
        }

        /*
        public void StartSpectator()
        {
            
        }

        public void StartScanView()
        {
            
        }
        */
        #endregion
    }
}