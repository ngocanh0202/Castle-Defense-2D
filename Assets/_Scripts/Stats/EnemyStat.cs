using Common2D;
using UnityEngine;
public enum EnemyStatType
{
    IdleSpeed,
    ChaseSpeed,
    MoveRange,
    RangeToChase,
    RangeToAttack
}
public class EnemyStat : ObjectStat
{
    [SerializeField] public Stat idleSpeed;
    [SerializeField] public Stat chaseSpeed;
    [SerializeField] public Stat moveRange;
    [SerializeField] public Stat rangeToChase;
    [SerializeField] public Stat rangeToAttack;


    public override void Awake()
    {
        EnemyStatScriptable enemyStatScriptable = ResourcesManager.GetEnemyStatScriptable();
        ApplyFromScriptable(enemyStatScriptable);
    }

    public override void Reset()
    {
        EnemyStatScriptable enemyStatScriptable = ResourcesManager.GetEnemyStatScriptable();
        ApplyFromScriptable(enemyStatScriptable);
    }

    public void ApplyFromScriptable(EnemyStatScriptable scriptable)
    {
        idleSpeed = new Stat(scriptable.idleSpeed);
        chaseSpeed = new Stat(scriptable.chaseSpeed);
        moveRange = new Stat(scriptable.moveRange);
        rangeToChase = new Stat(scriptable.rangeToChase);
        rangeToAttack = new Stat(scriptable.rangeToAttack);
    }

    public float GetStatValue(EnemyStatType statType)
    {
        return GetStat(statType).CurrentValue;
    }
    protected Stat GetStat(EnemyStatType statType)
    {
        return statType switch
        {
            EnemyStatType.IdleSpeed => idleSpeed,
            EnemyStatType.ChaseSpeed => chaseSpeed,
            EnemyStatType.MoveRange => moveRange,
            EnemyStatType.RangeToChase => rangeToChase,
            EnemyStatType.RangeToAttack => rangeToAttack,
            _ => throw new System.Exception($"Unknown stat type: {statType}")
        };
    }
}