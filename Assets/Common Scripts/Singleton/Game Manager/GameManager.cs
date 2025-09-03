using System;
using UnityEngine;

namespace Common2D.Singleton
{
    public class GameManager : Singleton<GameManager>
    {
        public GameManagerState CurrentGameState { get; private set; }
        public event Action<GameManagerState> OnGameStateChanged;

        protected override void Awake()
        {
            base.Awake();
        }
        public void ChangeState(GameManagerState newState)
        {
            if (CurrentGameState != newState)
            {
                CurrentGameState = newState;
                OnGameStateChanged?.Invoke(newState);
            }
        }

        public void ResetGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
