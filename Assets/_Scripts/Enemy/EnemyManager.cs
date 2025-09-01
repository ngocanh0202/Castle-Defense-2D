using System;
using System.Collections.Generic;
using Common2D;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    Action<TowerDefenseBehaviour> onSpawnEnemy;
    int spawnPointLength;
    bool topDowntoggle;
    [SerializeField] List<TowerDefenseBehaviour> spawnPoints = new List<TowerDefenseBehaviour>();
    [SerializeField] Transform enemyHolder;
    void Awake()
    {
        enemyHolder = transform.Find("Holder");
        TowerDefenseBehaviour[] spawnPointObjects = transform.GetComponentsInChildren<TowerDefenseBehaviour>();
        spawnPointLength = spawnPointObjects.Length;
        foreach (TowerDefenseBehaviour spawnPoint in spawnPointObjects)
        {
            spawnPoints.Add(spawnPoint);
        }

        EnemyBehaviour enemyBehaviour = ResourcesManager.GetEnemyPrefab().GetComponent<EnemyBehaviour>();
        ObjectPooler.InitObjectPooler<EnemyBehaviour>(
            KeyOfObjPooler.Enemy.ToString(),
            10,
            enemyBehaviour,
            (enemy) =>
            {
                enemy.transform.SetParent(enemyHolder);
            }
        );

        InvokeRepeating(nameof(HandleSpawnerEnemy), 2f, 1f);
    }

    void HandleSpawnerEnemy()
    {
        if (spawnPoints.Count == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Count);
        TowerDefenseBehaviour spawnPoint = spawnPoints[randomIndex];

        EnemyBehaviour enemy = ObjectPooler.GetObject<EnemyBehaviour>(KeyOfObjPooler.Enemy.ToString());
        if (enemy != null)
        {
            Vector2 vector2 = Random.insideUnitCircle * 1f;
            vector2.y = vector2.y + Random.Range(-2f, +2f);
            enemy.transform.position = spawnPoint.transform.position + (Vector3)vector2;
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            enemyStateMachine.IdleState.SetOriginalPosition(spawnPoint.transform.position);
            onSpawnEnemy?.Invoke(spawnPoint);
        }
    }

    public void StopInvokeSpawner()
    {
        CancelInvoke(nameof(HandleSpawnerEnemy));
    }
}
