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
        [Space]
        [SerializeField] private GameObject archibaldVisualPrefab;
        [SerializeField] private GameObject jacksonVisualPrefab;
        [SerializeField] private GameObject sheilaVisualPrefab;


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

                GameObject visual = null;
                switch (player.characterType.Value)
                {
                    case CharacterType.Jackson:
                        visual = Instantiate(jacksonVisualPrefab, character.transform);
                        break;
                    case CharacterType.Archibald:
                        visual = Instantiate(archibaldVisualPrefab, character.transform);
                        break;
                    case CharacterType.Sheila:
                        visual = Instantiate(sheilaVisualPrefab, character.transform);
                        break;
                }

                player.StartCharacter(character);
                visual.GetComponent<CharacterAnimation>().SetReference(character);

                character.transform.position = player.Position;
            }
        }

        #region Start Methods
        public void StartCharacter(CharacterType characterType)
        {
            GameObject character = Instantiate(localCharacterPrefab);
            LocalPlayer.StartCharacter(character);
            Camera.main.GetComponent<CameraFollow>().SetTarget(character.transform);

            GameObject visual = null;
            switch (characterType)
            {
                case CharacterType.Jackson:
                    visual = Instantiate(jacksonVisualPrefab, character.transform);
                    break;
                case CharacterType.Archibald:
                    visual = Instantiate(archibaldVisualPrefab, character.transform);
                    break;
                case CharacterType.Sheila:
                    visual = Instantiate(sheilaVisualPrefab, character.transform);
                    break;
            }

            visual.GetComponent<CharacterAnimation>().SetReference(character);
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