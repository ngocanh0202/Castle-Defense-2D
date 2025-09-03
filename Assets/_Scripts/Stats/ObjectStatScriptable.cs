using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Stat/ObjectStat", order = 1)]
public class ObjectStatScriptable : ScriptableObject
{
    [Header("ObjectStat Stats")]
    [SerializeField] public Stat vitality;
    [SerializeField] public Stat defense;
    [SerializeField] public Stat attack;
    [SerializeField] public Stat level;
}