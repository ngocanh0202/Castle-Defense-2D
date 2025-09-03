using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Stat/PlayerStat", order = 1)]
public class PlayerStatScriptable : ObjectStatScriptable
{
    [Header("PlayerStat Stats")]
    [SerializeField] public Stat speed;
    [SerializeField] public Stat experience;
}
