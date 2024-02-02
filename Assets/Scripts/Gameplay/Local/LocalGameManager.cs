using GrassGame.Gameplay.Synced;
using UnityEngine;

using GrassGame.Gameplay.Synced.Player.Enumerations;

namespace GrassGame.Gameplay.Local
{
    public class LocalGameManager : MonoBehaviour
    {
        public static LocalGameManager Instance { get; private set; }
        public static PlayerSync LocalPlayer { get; private set; }

        [Header("Prefabs: Local")]
        [SerializeField] private GameObject localCharacterPrefab;

        [Header("Prefabs: Readers")]
        [SerializeField] private GameObject characterReaderPrefab;

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

        public void AddSyncedPlayer(PlayerSync player)
        {
            Debug.Log($"Adding Synced Player {player}; Of Type {player.networkPlayerType.Value}");
            if (player.networkPlayerType.Value == PlayerType.NotSet)
            {
                player.networkPlayerType.OnValueChanged += player.OnTypeSet;
                return;
            }

            OnTypeSet(player);
        }

        public void OnTypeSet(PlayerSync player)
        {
            if (player.networkPlayerType.Value == PlayerType.Character)
            {
                GameObject character = Instantiate(characterReaderPrefab);
                player.StartCharacter(character);

                character.transform.position = player.Position;
            }
        }

        #region Start Methods
        public void StartCharacter()
        {
            GameObject character = Instantiate(localCharacterPrefab);
            LocalPlayer.StartCharacter(character);
            Camera.main.GetComponent<CameraFollow>().SetTarget(character.transform);
        }

        public void StartSpectator()
        {

        }

        public void StartScanView()
        {

        }
        
        #endregion
    }
}