using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoomState
{
    Boom Boom { get; set; }
    CircleCollider2D CircleCollider { get; set; }
    Transform Transform { get; set; }
    Animator Animator { get; set; }
}

public abstract class BoomBaseState : IBoomState, IState
{
    public CircleCollider2D CircleCollider { get; set; }
    public Animator Animator { get; set; }
    public Transform Transform { get ; set ; }
    public Boom Boom { get; set; }

    public BoomBaseState(
        CircleCollider2D circleCollider,
        Animator animator,
        Transform transform,
        Boom boom
    )
    {
        CircleCollider = circleCollider;
        Animator = animator;
        Transform = transform;
        Boom = boom;
    }
    abstract public void EnterState();
    abstract public void FixUpdateState();
    abstract public void OnCollision2D(Collision2D collision);
    abstract public void UpdateState();

    abstract public void OnTriggerEnter2D(Collider2D collision);
}
