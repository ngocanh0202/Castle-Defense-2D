using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    Gun enemyGun;
    Transform enemyTransform;
    Transform gunTransform;
    Transform target;
    
    private float stateCheckDelay = 0.2f;
    private float lastStateCheckTime;
    
    private float exitConfirmationTime = 0.4f; 
    private float currentExitTimer;
    private bool isConfirmingExit;

    public EnemyAttackState
    (
        EnemyStateMachine stateMachine,
        Animator animator,
        EnemyStat enemyStat,
        Gun enemyGun
    ) : base(stateMachine, animator, enemyStat)
    {
        enemyTransform = stateMachine.transform;
        gunTransform = enemyGun.transform;
        this.enemyGun = enemyGun;
    }

    public override void EnterState()
    {
        ResetStateChecking();
    }

    public void EnterState(Transform target)
    {
        enemyGun.SetStrategy(KeyGuns.Boom);
        this.target = target;
        ResetStateChecking();
    }

    private void ResetStateChecking()
    {
        lastStateCheckTime = 0f;
        currentExitTimer = 0f;
        isConfirmingExit = false;
    }

    public override void UpdateState()
    {
        try
        {
            
            Vector2 direction = (target.position - enemyTransform.position).normalized;
            gunTransform.right = direction;

            float distanceToTarget = Vector2.Distance(enemyTransform.position, target.position);

            HandleAttacking(distanceToTarget);

            if (Time.time - lastStateCheckTime >= stateCheckDelay)
            {
                CheckStateTransitions(distanceToTarget);
                lastStateCheckTime = Time.time;
            }

            UpdateConfirmationTimers();
        }catch
        {
            StateMachine.SwitchState(StateMachine.MainMissionState);
            return;
        }

    }

    private void HandleAttacking(float distanceToTarget)
    {
        bool canAttack = distanceToTarget <= EnemyStat.GetStatValue(EnemyStatType.RangeToAttack) &&
                        !enemyGun.isAttacking;

        if (canAttack)
        {
            enemyGun.Attack(false);
        }
    }

    private void CheckStateTransitions(float distanceToTarget)
    {
        if (distanceToTarget > EnemyStat.GetStatValue(EnemyStatType.RangeToAttack))
        {
            if (!isConfirmingExit)
            {
                StartExitConfirmation();
            }
        }
        else
        {
            CancelExitConfirmation();
        }
    }

    private void UpdateConfirmationTimers()
    {
        if (isConfirmingExit)
        {
            ConfirmExitTransition();
        }
    }

    private void StartExitConfirmation()
    {
        isConfirmingExit = true;
        currentExitTimer = 0f;
    }

    private void CancelExitConfirmation()
    {
        isConfirmingExit = false;
        currentExitTimer = 0f;
    }

    private void ConfirmExitTransition()
    {
        if (target == null)
        {
            StateMachine.SwitchState(StateMachine.MainMissionState);
            return;
        }

        float distanceToTarget = Vector2.Distance(enemyTransform.position, target.position);

        if (distanceToTarget <= EnemyStat.GetStatValue(EnemyStatType.RangeToChase))
        {
            StateMachine.SwitchState(StateMachine.ChaseState);
            EnemyChaseState chaseState = StateMachine.CurrentState as EnemyChaseState;
            chaseState?.EnterState(target);
        }
        
        ResetStateChecking();
    }

    public override void OnDrawGizmos()
    {
        if (enemyTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToAttack));

        if (isConfirmingExit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToAttack));
        }
        
        if (isConfirmingExit && currentExitTimer > 0)
        {
            float progress = currentExitTimer / exitConfirmationTime;
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, progress);
            Gizmos.DrawSphere(enemyTransform.position + Vector3.up * 2f, 0.2f);
        }
    }
}