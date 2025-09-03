using Common2D;
using Common2D.Singleton;
using UnityEngine;

public class TimeScaleManager : Singleton<TimeScaleManager>
{
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManagerState newState)
    {
        switch (newState)
        {
            case GameManagerState.Pause:
                Time.timeScale = 0;
                break;
            case GameManagerState.Resume:
                Time.timeScale = 1;
                break;
        }
    }
}