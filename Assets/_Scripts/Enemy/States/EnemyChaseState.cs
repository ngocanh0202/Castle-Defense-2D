using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    Transform target;
    Transform enemyTransform;
    
    private float stateCheckDelay = 0.2f;
    private float lastStateCheckTime;

    private bool isConfirmingAttack;
    private bool isConfirmingMainMissionState;
    private Collider2D tempTargetCollider; 

    public EnemyChaseState(EnemyStateMachine stateMachine, Animator animator, EnemyStat enemyStat) : base(stateMachine, animator, enemyStat)
    {
        enemyTransform = stateMachine.transform;
        target = null;
    }

    public override void EnterState()
    {
        target = null;
        ResetStateChecking();
    }

    public void EnterState(Transform tower)
    {
        target = tower;
        ResetStateChecking();
    }

    private void ResetStateChecking()
    {
        lastStateCheckTime = 0f;
        isConfirmingAttack = false;
        isConfirmingMainMissionState = false;
        tempTargetCollider = null;
    }

    public override void UpdateState()
    {
        if(target != null)
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, target.position, EnemyStat.GetStatValue(EnemyStatType.ChaseSpeed) * Time.deltaTime);
        else
            StateMachine.SwitchState(StateMachine.MainMissionState);

        if (Time.time - lastStateCheckTime >= stateCheckDelay)
        {
            CheckStateTransitions();
            lastStateCheckTime = Time.time;
        }

        UpdateConfirmationTimers();
    }

    private void CheckStateTransitions()
    {
        if (IsObjectInRangeToAttack(out Collider2D collider))
        {
            if (!isConfirmingAttack)
            {
                StartAttackConfirmation(collider);
                CancelMainMissionStateConfirmation();
            }
        }
        else
        {
            CancelAttackConfirmation();

            if (target == null) return;
            float distanceToTarget = Vector2.Distance(enemyTransform.position, target.position);
            if (distanceToTarget > EnemyStat.GetStatValue(EnemyStatType.RangeToChase))
            {
                if (!isConfirmingMainMissionState)
                {
                    StartMainMissionStateConfirmation();
                }
            }
            else
            {
                CancelMainMissionStateConfirmation();
            }
        }
    }

    private void UpdateConfirmationTimers()
    {
        if (isConfirmingAttack)
        {
            ConfirmAttackTransition();
        }

        if (isConfirmingMainMissionState && !isConfirmingAttack)
        {
            ConfirmMainMissionStateTransition();
        }
    }

    private void StartAttackConfirmation(Collider2D targetCollider)
    {
        isConfirmingAttack = true;
        tempTargetCollider = targetCollider;
    }

    private void CancelAttackConfirmation()
    {
        isConfirmingAttack = false;
        tempTargetCollider = null;
    }

    private void ConfirmAttackTransition()
    {
        if (tempTargetCollider != null)
        {
            StateMachine.SwitchState(StateMachine.AttackState);
            EnemyAttackState enemyAttackState = StateMachine.CurrentState as EnemyAttackState;
            enemyAttackState?.EnterState(tempTargetCollider.transform);
        }
        ResetStateChecking();
    }

    private void StartMainMissionStateConfirmation()
    {
        isConfirmingMainMissionState = true;
    }

    private void CancelMainMissionStateConfirmation()
    {
        isConfirmingMainMissionState = false;
    }

    private void ConfirmMainMissionStateTransition()
    {
        StateMachine.SwitchState(StateMachine.MainMissionState);
        EnemyMainMissionState enemyMainMissionState = StateMachine.CurrentState as EnemyMainMissionState;
        enemyMainMissionState?.EnterState();
        ResetStateChecking();
    }

    private bool IsObjectInRangeToAttack(out Collider2D colliderObj)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToAttack));
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
        if (enemyTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToAttack));

        if (isConfirmingAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToAttack));
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyTransform.position, EnemyStat.GetStatValue(EnemyStatType.RangeToChase));

        if (target != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(enemyTransform.position, target.position);
        }
    }
}