using Common2D;
using UnityEngine;
public enum TowerStatType
{
    Range,
    Direction
}
public class TowerStat : ObjectStat
{
    [SerializeField] public Stat range;
    [SerializeField] public string direction;

    public override void Awake()
    {
        // TowerStatScriptable TowerStatScriptable = ResourcesManager.GetTowerStatScriptable("TowerStat");
        // ApplyFromScriptable(TowerStatScriptable);
    }
    public override void Reset()
    {
        TowerStatScriptable TowerStatScriptable = ResourcesManager.GetTowerStatScriptable("TowerStat");
        ApplyFromScriptable(TowerStatScriptable);
    }

    [ContextMenu("Set Main Stats")]
    public void SetMainTowerStats()
    {
        TowerStatScriptable TowerStatScriptable = ResourcesManager.GetTowerStatScriptable("MainTowerStat");
        ApplyFromScriptable(TowerStatScriptable);
    }

    public void ApplyFromScriptable(TowerStatScriptable scriptable)
    {
        vitality = scriptable.vitality;
        defense = scriptable.defense;
        attack = scriptable.attack;
        level = scriptable.level;
        direction = scriptable.direction;
        range = scriptable.range;
    }

    public T GetStatValue<T>(TowerStatType statType)
    {
        if (typeof(T) == typeof(string) && statType == TowerStatType.Direction)
            return (T)(object)direction;
        else if (typeof(T) == typeof(float) && statType == TowerStatType.Range)
            return (T)(object)range.CurrentValue;
        else
            throw new System.NotImplementedException($"GetStatValue is not implemented for type {typeof(T)}");
    }
}