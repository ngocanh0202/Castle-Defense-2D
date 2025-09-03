using System;
using System.Collections.Generic;
using Common2D;
using Common2D.Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerManager : Singleton<EnemySpawnerManager>
{
    Action<TowerDefenseBehaviour> onSpawnEnemy;
    int spawnPointLength;
    bool topDowntoggle;
    [SerializeField] List<TowerDefenseBehaviour> spawnPoints = new List<TowerDefenseBehaviour>();
    [SerializeField] Transform enemyHolder;
    protected override void Awake()
    {
        base.Awake();
        InitTowerEnemy();
        EnemyBehaviour enemyBehaviour = ResourcesManager.GetEnemyPrefab().GetComponent<EnemyBehaviour>();
        ObjectPooler.Instance.InitObjectPooler<EnemyBehaviour>(
            KeyOfObjPooler.Enemy.ToString(),
            10,
            enemyBehaviour
        );
        TowerGridManager.Instance.onSetTowerEnemy += HandleSetTowerEnemy;
    }

    protected void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManagerState newState)
    {
        if (newState == GameManagerState.GameOver)
        {
            ReceiveDamageBehaviour[] receiveDamageBehaviours = FindObjectsByType<ReceiveDamageBehaviour>(FindObjectsSortMode.None);
            foreach (var enemy in receiveDamageBehaviours)
            {
                enemy.IsInvulnerable = true;
            }

            EnemyStateMachine[] enemyStateMachines = FindObjectsByType<EnemyStateMachine>(FindObjectsSortMode.None);
            foreach (var enemy in enemyStateMachines)
            {
                enemy.SwitchState(enemy.IdleState);
            }

            StopInvokeSpawner();
        }
    }

    void InitTowerEnemy()
    {
        enemyHolder = transform.Find("Holder");
        TowerDefenseBehaviour[] spawnPointObjects = transform.GetComponentsInChildren<TowerDefenseBehaviour>();
        spawnPointLength = spawnPointObjects.Length;
        if(spawnPointLength > 0)
        {
            foreach (TowerDefenseBehaviour spawnPoint in spawnPointObjects)
            {
                spawnPoints.Add(spawnPoint);
                spawnPoint.onTowerDisable += OnTowerDisable;
                spawnPoint.onTowerEnable += OnTowerEnable;
            }
            InvokeRepeating(nameof(HandleSpawnerEnemy), 2f, 1f);
        }
    }

    void HandleSetTowerEnemy()
    {
        StopInvokeSpawner();
        InitTowerEnemy();
    }

    void HandleSpawnerEnemy()
    {
        if (spawnPointLength == 0) return;

        int randomIndex = Random.Range(0, spawnPointLength);
        TowerDefenseBehaviour spawnPoint = spawnPoints[randomIndex];

        EnemyBehaviour enemy = ObjectPooler.Instance.GetObject<EnemyBehaviour>(KeyOfObjPooler.Enemy.ToString());
        if (enemy != null)
        {
            Vector2 vector2 = Random.insideUnitCircle * 1f;
            topDowntoggle = !topDowntoggle;
            vector2.y = vector2.y + (topDowntoggle ? 2f : -2f);
            enemy.transform.position = spawnPoint.transform.position + (Vector3)vector2;
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            enemyStateMachine.IdleState.SetOriginalPosition(spawnPoint.transform.position);
            onSpawnEnemy?.Invoke(spawnPoint);
        }
    }

    public void OnTowerEnable(TowerDefenseBehaviour tower)
    {
        if (!spawnPoints.Contains(tower))
        {
            spawnPoints.Add(tower);
            spawnPointLength++;
        }
    }

    public void OnTowerDisable(TowerDefenseBehaviour tower)
    {
        spawnPoints.Remove(tower);
        spawnPointLength--;
    }

    public void StopInvokeSpawner()
    {
        CancelInvoke(nameof(HandleSpawnerEnemy));
    }
}
