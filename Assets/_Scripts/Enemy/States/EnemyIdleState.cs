using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private Transform transform;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float moveRange;
    private float idleSpeed;
    private float rangeToChase = 5f;
    
    private float stateCheckDelay = 0.2f; 
    private float lastStateCheckTime;
    
    private float chaseConfirmationTime = 0.5f; 
    private float currentChaseTimer;
    private bool isConfirmingChase;
    private Collider2D potentialTarget;
    
    private float waitTime = 2f;
    private float currentWaitTime;
    private float positionThreshold = 0.1f; 

    public EnemyIdleState(EnemyStateMachine stateMachine, Animator animator, float moveRange, float idleSpeed, float rangeToChase) : base(stateMachine, animator)
    {
        transform = stateMachine.transform;
        originalPosition = stateMachine.transform.position;
        this.moveRange = moveRange;
        this.idleSpeed = idleSpeed;
        this.rangeToChase = rangeToChase;
    }

    public override void EnterState()
    {
        if (targetPosition == Vector3.zero || Vector3.Distance(transform.position, originalPosition) <= moveRange)
        {
            targetPosition = transform.position; 
        }
        ResetStateChecking();
        ResetIdleMovement();
    }

    private void ResetStateChecking()
    {
        lastStateCheckTime = 0f;
        currentChaseTimer = 0f;
        isConfirmingChase = false;
        potentialTarget = null;
    }

    private void ResetIdleMovement()
    {
        currentWaitTime = 0f;
    }

    public override void UpdateState()
    {
        HandleIdleMovement();

        if (Time.time - lastStateCheckTime >= stateCheckDelay)
        {
            CheckForChaseTargets();
            lastStateCheckTime = Time.time;
        }

        UpdateConfirmationTimers();
    }

    private void HandleIdleMovement()
    {
        float distanceToTarget = Vector3.Distance(StateMachine.transform.position, targetPosition);

        if (distanceToTarget > positionThreshold)
        {
            StateMachine.transform.position = Vector2.MoveTowards(
                StateMachine.transform.position,
                targetPosition,
                Time.deltaTime * idleSpeed
            );
            currentWaitTime = 0f;
        }
        else
        {
            currentWaitTime += Time.deltaTime;
            if (currentWaitTime >= waitTime)
            {
                ChooseNewIdlePosition();
                currentWaitTime = 0f;
            }
        }
    }

    private void ChooseNewIdlePosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * moveRange;
        Vector3 newPosition = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
        
        float minDistance = moveRange * 0.3f; 
        while (Vector3.Distance(newPosition, StateMachine.transform.position) < minDistance)
        {
            randomOffset = Random.insideUnitCircle * moveRange;
            newPosition = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
        }
        
        targetPosition = newPosition;
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

    private void StartChaseConfirmation(Collider2D target)
    {
        if (potentialTarget != target)
        {
            isConfirmingChase = true;
            currentChaseTimer = 0f;
            potentialTarget = target;
        }
    }

    private void ResetChaseConfirmation()
    {
        currentChaseTimer = 0f;
        isConfirmingChase = false;
        potentialTarget = null;
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

    public void SetOriginalPosition(Vector3 position)
    {
        originalPosition = position;
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

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originalPosition, moveRange);

        if (targetPosition != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(targetPosition, 0.3f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetPosition);
        }

        if (isConfirmingChase && currentChaseTimer > 0)
        {
            float progress = currentChaseTimer / chaseConfirmationTime;
            Gizmos.color = Color.Lerp(Color.green, Color.red, progress);
            Gizmos.DrawSphere(transform.position + Vector3.up * 1.5f, 0.15f);
        }
    }
}