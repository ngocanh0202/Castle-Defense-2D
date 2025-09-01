using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainMissionState : EnemyBaseState
{
    Transform transform;
    Transform targetTransform;
    Vector3 targetPosition;
    float rangeToChase;
    float stateCheckDelay = 0.2f; 
    float lastStateCheckTime;
    
    float chaseConfirmationTime = 0.5f; 
    float currentChaseTimer;
    bool isConfirmingChase;
    Collider2D potentialTarget;
    
    float positionThreshold = 0.5f; 

    float idleSpeed = 1f;
    public EnemyMainMissionState(
        EnemyStateMachine stateMachine, Transform targetTransform, Animator animator, float rangeToChase, float idleSpeed) :
        base(stateMachine, animator)
    {
        transform = stateMachine.transform;
        this.targetTransform = targetTransform;
        targetPosition = targetTransform.position;
        this.rangeToChase = rangeToChase;
        this.idleSpeed = idleSpeed;
    }

    public override void EnterState()
    {
        if(targetTransform == null || !targetTransform.gameObject.activeSelf)
        {
            StateMachine.SwitchState(StateMachine.IdleState);
            return;
        }
        ResetStateChecking();
    }

    public override void UpdateState()
    {
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget > positionThreshold)
        {
            StateMachine.transform.position = Vector2.MoveTowards(
                StateMachine.transform.position,
                targetPosition,
                Time.deltaTime * idleSpeed
            );
        }
        else
        {
            StateMachine.SwitchState(StateMachine.IdleState);
            return;
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
        if (IsObjectInRangeToChase(rangeToChase, out Collider2D collider))
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
            
            PlayerReceiveDamageBehaviour receiveDamageBehaviour = collider.GetComponent<PlayerReceiveDamageBehaviour>();
            if (receiveDamageBehaviour != null)
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
        Gizmos.DrawWireSphere(transform.position, rangeToChase);

        if (isConfirmingChase)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangeToChase);
        }
    }
}
