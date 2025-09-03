using UnityEngine;

public interface IEnemyState
{
    EnemyStateMachine StateMachine { get; set; }
    Animator Animator { get; set; }
}

public abstract class EnemyBaseState : IEnemyState, IState, IDrawGizmos
{
    public EnemyStateMachine StateMachine { get; set ;}
    public Animator Animator { get; set ;}
    public EnemyStat EnemyStat { get; set; }
    public EnemyBaseState(EnemyStateMachine stateMachine, Animator animator, EnemyStat enemyStat)
    {
        this.StateMachine = stateMachine;
        this.Animator = animator;
        this.EnemyStat = enemyStat;
    }

    public virtual void EnterState() {}

    public virtual void FixUpdateState() {}

    public virtual void OnCollision2D(Collision2D collision) {}

    public virtual void UpdateState() {}

    public virtual void OnTriggerEnter2D(Collider2D collision) {}

    public virtual void OnDrawGizmos() {}
}
