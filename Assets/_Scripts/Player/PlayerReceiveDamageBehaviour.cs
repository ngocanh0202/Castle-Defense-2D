using Common2D.Singleton;
using UnityEngine;

[RequireComponent(typeof(PlayerStat))]
public class PlayerReceiveDamageBehaviour : ReceiveDamageBehaviour
{
    public override void Die()
    {
        base.Die();
        GameManager.Instance.ChangeState(GameManagerState.GameOver);
    }
}
