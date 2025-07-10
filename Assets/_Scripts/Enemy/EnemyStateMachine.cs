using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyStateMachine : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public EnemyBaseState CurrentState;
    public EnemyIdleState IdleState;
    public EnemyMoveState MoveState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyReturnState ReturnState;
    public EnemyDeathState DeathState;
    public EnemyDamgedState DamagedState;

    void Start()
    {
        InitializeComponents();
        IdleState = new EnemyIdleState(this, animator);
        MoveState = new EnemyMoveState(this, animator);
        ChaseState = new EnemyChaseState(this, animator);
        AttackState = new EnemyAttackState(this, animator);
        ReturnState = new EnemyReturnState(this, animator);
        DeathState = new EnemyDeathState(this, animator);
        DamagedState = new EnemyDamgedState(this, animator);

        CurrentState = IdleState;
        CurrentState.EnterState();
    }

    void Update()
    {
        CurrentState.UpdateState();
    }
    void FixedUpdate()
    {
        CurrentState.FixUpdateState();
    }

    public void SwitchState(EnemyBaseState newState)
    {
        CurrentState = newState;
        CurrentState.EnterState();
    }

    private void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the enemy!");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the enemy!");
        }
    }

    public IEnumerator ChangeColorCoroutine(Color originalColor, Color targetColor, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
