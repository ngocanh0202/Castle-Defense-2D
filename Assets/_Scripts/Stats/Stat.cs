using UnityEngine;
using System;

[Serializable]
public struct Stat
{
    [SerializeField] float currentValue;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    
    public float CurrentValue => currentValue;
    public float MinValue => minValue;
    public float MaxValue => maxValue;

    public Stat(Stat newStat)
    {
        currentValue = newStat.currentValue;
        minValue = newStat.minValue;
        maxValue = newStat.maxValue;
    }
    
    public void SetCurrentValue(float value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
    }
}
