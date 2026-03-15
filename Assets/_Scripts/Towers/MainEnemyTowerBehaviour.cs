using Common2D.Singleton;
using UnityEngine;

[RequireComponent(typeof(TowerDefenseBehaviour))]
[RequireComponent(typeof(EnemyTowerBehaviour))]
public class MainEnemyTowerBehaviour : MonoBehaviour
{
    private EnemyTowerBehaviour enemyTowerBehaviour;
    private ReceiveDamageBehaviour receiveDamageBehaviour;

    void Awake()
    {
        enemyTowerBehaviour = GetComponent<EnemyTowerBehaviour>();
        receiveDamageBehaviour = GetComponent<ReceiveDamageBehaviour>();
    }

    void OnEnable()
    {
        receiveDamageBehaviour.OnDie += HandleDeath;
    }

    void OnDisable()
    {
        receiveDamageBehaviour.OnDie -= HandleDeath;
    }

    private void HandleDeath(Transform transform)
    {
        TriggerVictory();
    }

    private void TriggerVictory()
    {
        GameManager.Instance.ChangeState(GameManagerState.Victory);
    }
}
