using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomShootState : BoomBaseState
{
    public float speed;
    public float collisionRadius;
    Vector3 transformScale;

    public BoomShootState(CircleCollider2D circleCollider, Animator animator, Transform transform, Boom boom, float speed) : base(circleCollider, animator, transform, boom)
    {
        this.speed = speed;
        this.collisionRadius = circleCollider.radius;
        this.transformScale = transform.localScale;
    }

    public override void EnterState()
    {
        Transform.localScale = transformScale;
    }

    public override void FixUpdateState()
    {
        
    }

    public override void OnCollision2D(Collision2D collision)
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public override void UpdateState()
    {
        Transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
