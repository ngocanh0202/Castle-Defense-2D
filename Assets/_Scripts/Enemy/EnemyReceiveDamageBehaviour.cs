using System;
using System.ComponentModel;
using UnityEngine;

public class EnemyReceiveDamageBehaviour : ReceiveDamageBehaviour
{
    Action<Transform> OnDie;

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
        OnDie?.Invoke(this.transform);
        base.Die();
    }
}
