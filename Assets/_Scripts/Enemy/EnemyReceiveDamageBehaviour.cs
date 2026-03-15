public class EnemyReceiveDamageBehaviour : ReceiveDamageBehaviour
{
    void OnEnable()
    {
        InitializeComponents();
        ResetHealth();
        if (OnDie == null)
        {
            OnDie += ItemDropSystem.Instance.OnEnemyDie;
        }
    }
    public override void Die()
    {
        base.Die();
        if (OnDie != null)
        {
            OnDie -= ItemDropSystem.Instance.OnEnemyDie;
        }
    }
}
