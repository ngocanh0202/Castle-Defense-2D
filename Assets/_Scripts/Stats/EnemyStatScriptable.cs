
using UnityEngine;
[CreateAssetMenu(fileName = "Stat", menuName = "Stat/Enemy", order = 2)]
public class EnemyStatScriptable : ObjectStatScriptable
{
    [Header("Enemy Stats")]
    [SerializeField] public Stat speed;
    [SerializeField] public Stat experience;
    [SerializeField] public Stat idleSpeed;
    [SerializeField] public Stat chaseSpeed;
    [SerializeField] public Stat moveRange;
    [SerializeField] public Stat rangeToChase;
    [SerializeField] public Stat rangeToAttack;
}
