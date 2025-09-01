using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    Transform target;
    Transform enemyTransform;
    public float rangeToChase; 
    float chaseSpeed;
    float rangeToAttack = 3f;
    
    private float stateCheckDelay = 0.2f;
    private float lastStateCheckTime;

    private bool isConfirmingAttack;
    private bool isConfirmingMainMissionState;
    private Collider2D tempTargetCollider; 

    public EnemyChaseState(EnemyStateMachine stateMachine, Animator animator, float rangeToChase, float chaseSpeed, float rangeToAttack) : base(stateMachine, animator)
    {
        enemyTransform = stateMachine.transform;
        target = null;
        this.rangeToChase = rangeToChase;
        this.chaseSpeed = chaseSpeed;
        this.rangeToAttack = rangeToAttack;
    }

    public override void EnterState()
    {
        target = null;
        ResetStateChecking();
    }

    public void EnterState(Transform player)
    {
        target = player;
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
        if (target == null || target.gameObject.activeSelf == false)
        {
            StateMachine.SwitchState(StateMachine.IdleState);
            return;
        }

        enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, target.position, chaseSpeed * Time.deltaTime);
        
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
            
            float distanceToTarget = Vector2.Distance(enemyTransform.position, target.position);
            if (distanceToTarget > rangeToChase)
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyTransform.position, rangeToAttack);
        foreach (var collider in colliders)
        {
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
        if (enemyTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemyTransform.position, rangeToAttack);
        
        if (isConfirmingAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemyTransform.position, rangeToAttack);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyTransform.position, rangeToChase);
        
        if (target != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(enemyTransform.position, target.position);
        }
    }
}