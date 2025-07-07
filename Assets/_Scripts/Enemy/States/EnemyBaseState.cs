using UnityEngine;

public interface IEnemyState
{
    EnemyStateMachine StateMachine { get; set; }
    Animator Animator { get; set; }
}

public abstract class EnemyBaseState : IEnemyState, IState
{
    public EnemyStateMachine StateMachine { get; set ;}
    public Animator Animator { get; set ;}
    public EnemyBaseState(EnemyStateMachine stateMachine, Animator animator)
    {
        this.StateMachine = stateMachine;
        this.Animator = animator;
    }

    public abstract void EnterState();

    public abstract void FixUpdateState();

    public abstract void OnCollision2D(Collision2D collision);

    public abstract void UpdateState();
}
