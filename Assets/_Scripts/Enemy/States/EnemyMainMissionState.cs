using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainMissionState : EnemyBaseState
{
    Transform transform;
    Transform mainTargetTransform;

    float stateCheckDelay = 0.2f; 
    float lastStateCheckTime;
    
    float chaseConfirmationTime = 0.5f; 
    float currentChaseTimer;
    bool isConfirmingChase;

    Collider2D potentialTarget;
    float positionThreshold = 0.5f; 
    public EnemyMainMissionState(
        EnemyStateMachine stateMachine, Transform mainTargetTransform, Animator animator, EnemyStat enemyStat) :
        base(stateMachine, animator, enemyStat)
    {
        transform = stateMachine.transform;
        this.mainTargetTransform = mainTargetTransform;
    }

    public override void EnterState()
    {
        ResetStateChecking();
    }

    public override void UpdateState()
    {
        if (mainTargetTransform == null) return;
        float distanceToTarget = Vector2.Distance(transform.position, mainTargetTransform.position);
        if (distanceToTarget > positionThreshold)
        {
            StateMachine.transform.position = Vector2.MoveTowards(
                StateMachine.transform.position,
                mainTargetTransform.position,
                Time.deltaTime * EnemyStat.GetStatValue(EnemyStatType.IdleSpeed)
            );
        }

        if (Time.time - lastStateCheckTime >= stateCheckDelay)
        {
            CheckForChaseTargets();
            lastStateCheckTime = Time.time;
        }

        UpdateConfirmationTimers();
    }

    private void ResetStateChecking()
    {
        lastStateCheckTime = 0f;
        currentChaseTimer = 0f;
        isConfirmingChase = false;
        potentialTarget = null;
    }

    private void CheckForChaseTargets()
    {
        if (IsObjectInRangeToChase(EnemyStat.GetStatValue(EnemyStatType.RangeToChase), out Collider2D collider))
        {
            if (!isConfirmingChase || potentialTarget != collider)
            {
                StartChaseConfirmation(collider);
            }
        }
        else
        {
            ResetChaseConfirmation();
        }
    }

    private void StartChaseConfirmation(Collider2D target)
    {
        if (potentialTarget != target)
        {
            isConfirmingChase = true;
            currentChaseTimer = 0f;
            potentialTarget = target;
        }
    }

    private void UpdateConfirmationTimers()
    {
        if (isConfirmingChase)
        {
            currentChaseTimer += Time.deltaTime;
            if (currentChaseTimer >= chaseConfirmationTime)
            {
                ConfirmChaseTransition();
            }
        }
    }

    private void ConfirmChaseTransition()
    {
        if (potentialTarget != null)
        {
            StateMachine.SwitchState(StateMachine.ChaseState);
            EnemyChaseState enemyChaseState = StateMachine.CurrentState as EnemyChaseState;
            enemyChaseState?.EnterState(potentialTarget.transform);
        }
        ResetChaseConfirmation();
    }

    private void ResetChaseConfirmation()
    {
        currentChaseTimer = 0f;
        isConfirmingChase = false;
        potentialTarget = null;
    }

    bool IsObjectInRangeToChase(float range, out Collider2D colliderObj)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var collider in colliders)
        {
            if (collider == null) continue;

            if (collider.CompareTag("Player"))
            {
                colliderObj = collider;
                return true;
            }
        }
        colliderObj = null;
        return false;
    }
    
    public override void OnDrawGizmos()
    {
        if (transform == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToChase));

        if (isConfirmingChase)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToChase));
        }
    }
}
