using System.Collections.Generic;
using Common2D;
using Common2D.Singleton;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    [Header("Wave Settings")]
    [SerializeField] private float waveInterval = 10f;
    [SerializeField] private bool wavesActive = false;
    [SerializeField] private int currentWave = 0;

    private float waveTimer;
    private List<EnemyTowerBehaviour> enemyTowers;

    protected override void Awake()
    {
        base.Awake();
        enemyTowers = new List<EnemyTowerBehaviour>();
        waveTimer = 0f;
    }

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameManagerState newState)
    {
        if (newState == GameManagerState.GameOver || newState == GameManagerState.Victory)
        {
            StopWaves();
        }
    }

    void Update()
    {
        if (!wavesActive) return;

        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            SpawnWave();
            waveTimer = 0f;
        }
    }

    public void StartWaves()
    {
        wavesActive = true;
        waveTimer = waveInterval;
        currentWave = 0;
    }

    public void StopWaves()
    {
        wavesActive = false;
    }

    public void PauseWaves()
    {
        wavesActive = false;
    }

    public void ResumeWaves()
    {
        if (GameManager.Instance.CurrentGameState != GameManagerState.GameOver &&
            GameManager.Instance.CurrentGameState != GameManagerState.Victory)
        {
            wavesActive = true;
        }
    }

    private void SpawnWave()
    {
        currentWave++;
        foreach (EnemyTowerBehaviour tower in enemyTowers)
        {
            if (tower != null && tower.gameObject.activeInHierarchy)
            {
                tower.ResetSpawnTimer();
                tower.SpawnEnemy();
            }
        }
    }

    public void RegisterEnemyTower(EnemyTowerBehaviour tower)
    {
        if (!enemyTowers.Contains(tower))
        {
            enemyTowers.Add(tower);
        }
    }

    public void UnregisterEnemyTower(EnemyTowerBehaviour tower)
    {
        enemyTowers.Remove(tower);
    }

    public void SetWaveInterval(float interval)
    {
        waveInterval = Mathf.Max(1f, interval);
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public bool IsWavesActive()
    {
        return wavesActive;
    }
}
