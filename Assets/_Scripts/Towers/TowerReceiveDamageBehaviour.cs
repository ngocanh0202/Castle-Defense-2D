using Common2D.Singleton;
using UnityEngine;

[RequireComponent(typeof(TowerStat))]
public class TowerReceiveDamageBehaviour : ReceiveDamageBehaviour
{
    public override void Die()
    {
        base.Die();
        if(gameObject.name == "MainTower")
        {
            GameManager.Instance.ChangeState(GameManagerState.GameOver);
        }
    }
}
