using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Stat/TowerStat", order = 1)]
public class TowerStatScriptable : ObjectStatScriptable
{
    [Header("TowerStat Stats")]
    [SerializeField] public Stat range;
    [SerializeField] public string direction = "0,0,0,0,0,0,0,0";
}
