using Common2D.Singleton;
using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public class MainTowerBehaviour : ReceiveDamageBehaviour
{
    public override void Die()
    {
        base.Die();
        GameManager.Instance.ChangeState(GameManagerState.GameOver);
    }
}
