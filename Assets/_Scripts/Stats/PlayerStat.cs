using System;
using Common2D;
using UnityEngine;
public enum PlayerStatType
{
    Speed,
    Experience
}
public class PlayerStat : ObjectStat
{
    [SerializeField] public Stat speed;
    [SerializeField] public Stat experience;

    public override void Awake()
    {
        PlayerStatScriptable playerStatScriptable = ResourcesManager.GetPlayerStatScriptable();
        ApplyFromScriptable(playerStatScriptable);
    }

    public override void Reset()
    {
        PlayerStatScriptable playerStatScriptable = ResourcesManager.GetPlayerStatScriptable();
        ApplyFromScriptable(playerStatScriptable);
    }

    public void ApplyFromScriptable(PlayerStatScriptable scriptable)
    {
        vitality = new Stat(scriptable.vitality);
        defense = new Stat(scriptable.defense);
        attack = new Stat(scriptable.attack);
        level = new Stat(scriptable.level);
        speed = new Stat(scriptable.speed);
        experience = new Stat(scriptable.experience);
    }

    public void SetExperienceValue(float value, Action<float> onPlusEXP = null, Action onLevelUp = null)
    {
        float newExperience = experience.CurrentValue + value / level.CurrentValue;
        onPlusEXP?.Invoke(newExperience - experience.CurrentValue);
        experience.SetCurrentValue(newExperience);
        if (experience.CurrentValue == experience.MaxValue)
        {
            LevelUp();
            onLevelUp?.Invoke();
        }
    }

    public void LevelUp()
    {
        attack.SetCurrentValue(attack.CurrentValue + level.CurrentValue);
        level.SetCurrentValue(level.CurrentValue + 1);
        experience.SetCurrentValue(0);
    }

    public float GetStatValue(PlayerStatType statType)
    {
        return GetStat(statType).CurrentValue;
    }
    protected Stat GetStat(PlayerStatType statType)
    {
        return statType switch
        {
            PlayerStatType.Speed => speed,
            PlayerStatType.Experience => experience,
            _ => throw new System.Exception($"Unknown stat type: {statType}")
        };
    }
}