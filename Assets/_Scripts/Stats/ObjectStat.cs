using System;
using Common2D;
using UnityEngine;
public enum StatType
{
    Level,
    Vitality,
    Defense,
    Attack,

}
public class ObjectStat : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected Stat vitality;
    [SerializeField] protected Stat defense;
    [SerializeField] protected Stat attack;
    [SerializeField] protected Stat level;

    public virtual void Awake()
    {
        ObjectStatScriptable objectStatScriptable = ResourcesManager.GetObjectStatScriptable();
        ApplyFromScriptable(objectStatScriptable);
    }

    public virtual void Reset()
    {
        ObjectStatScriptable objectStatScriptable = ResourcesManager.GetObjectStatScriptable();
        ApplyFromScriptable(objectStatScriptable);
    }
    public float GetStatValue(StatType statType)
    {
        return GetStat(statType).CurrentValue;
    }

    public void ApplyFromScriptable(ObjectStatScriptable scriptable)
    {
        vitality = new Stat(scriptable.vitality);
        defense = new Stat(scriptable.defense);
        attack = new Stat(scriptable.attack);
        level = new Stat(scriptable.level);
    }

    protected Stat GetStat(StatType statType)
    {
        return statType switch
        {
            StatType.Vitality => vitality,
            StatType.Defense => defense,
            StatType.Attack => attack,
            StatType.Level => level,
            _ => throw new Exception($"Unknown stat type: {statType}")
        };
    }
}
