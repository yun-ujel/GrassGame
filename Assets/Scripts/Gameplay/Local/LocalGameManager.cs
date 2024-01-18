using GrassGame.Gameplay.Synced;
using UnityEngine;

namespace GrassGame.Gameplay.Local
{
    public class LocalGameManager : MonoBehaviour
    {
        public static LocalGameManager Instance { get; private set; }
        public static PlayerSync LocalPlayer { get; private set; }

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

        public void StartPlayer()
        {

        }
    }
}