using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine stateMachine, Animator animator) : base(stateMachine, animator)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entering Idle State");
    }

    public override void FixUpdateState()
    {
        
    }

    public override void OnCollision2D(Collision2D collision)
    {
        
    }

    public override void UpdateState()
    {
        
    }

}
