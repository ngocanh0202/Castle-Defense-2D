using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private Transform transform;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    
    private float waitTime = 2f;
    private float currentWaitTime;
    private float positionThreshold = 0.1f; 

    public EnemyIdleState(EnemyStateMachine stateMachine, Animator animator, EnemyStat enemyStat) : base(stateMachine, animator, enemyStat)
    {
        transform = stateMachine.transform;
        originalPosition = stateMachine.transform.position;
    }

    public override void EnterState()
    {
        if (targetPosition == Vector3.zero || Vector3.Distance(transform.position, originalPosition) <= EnemyStat.GetStatValue(EnemyStatType.MoveRange))
        {
            targetPosition = transform.position; 
        }
        ResetIdleMovement();
    }


    private void ResetIdleMovement()
    {
        currentWaitTime = 0f;
    }

    public override void UpdateState()
    {
        HandleIdleMovement();
    }

    private void HandleIdleMovement()
    {
        float distanceToTarget = Vector3.Distance(StateMachine.transform.position, targetPosition);

        if (distanceToTarget > positionThreshold)
        {
            StateMachine.transform.position = Vector2.MoveTowards(
                StateMachine.transform.position,
                targetPosition,
                Time.deltaTime * EnemyStat.GetStatValue(EnemyStatType.IdleSpeed)
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
        Vector2 randomOffset = Random.insideUnitCircle * EnemyStat.GetStatValue(EnemyStatType.MoveRange);
        Vector3 newPosition = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

        float minDistance = EnemyStat.GetStatValue(EnemyStatType.MoveRange) * 0.3f;
        while (Vector3.Distance(newPosition, StateMachine.transform.position) < minDistance)
        {
            randomOffset = Random.insideUnitCircle * EnemyStat.GetStatValue(EnemyStatType.MoveRange);
            newPosition = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
        }
        
        targetPosition = newPosition;
    }

    public void SetOriginalPosition(Vector3 position)
    {
        originalPosition = position;
    }
    
}