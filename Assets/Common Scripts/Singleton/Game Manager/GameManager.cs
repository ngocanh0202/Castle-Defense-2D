using System;

namespace Common2D.Singleton
{
    public class GameManager : Singleton<GameManager>
    {
        public GameManagerState CurrentGameState { get; private set; }
        public event Action<GameManagerState> OnGameStateChanged;
        protected override void Awake()
        {
            base.Awake();
            // Todo: Add initialization code here if needed
        }
        public void ChangeState(GameManagerState newState)
        {
            if (CurrentGameState != newState)
            {
                CurrentGameState = newState;
                OnGameStateChanged?.Invoke(newState);

                // Todo: Add any additional logic for state changes here
                
            }
        }
    }
}
