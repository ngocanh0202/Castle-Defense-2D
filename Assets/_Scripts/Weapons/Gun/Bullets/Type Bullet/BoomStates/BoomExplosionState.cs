using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomExplosionState : BoomBaseState
{
    float explosionRadius;
    float explosionSpeed;

    public BoomExplosionState(CircleCollider2D circleCollider, Animator animator, Transform transform, Boom boom) : base(circleCollider, animator, transform, boom)
    {
        explosionRadius = circleCollider.radius + 1.25f;
        explosionSpeed = 1f;
    }

    public override void EnterState()
    {
        
    }

    public override void FixUpdateState()
    {
        
    }

    public override void OnCollision2D(Collision2D collision)
    {
        
    }

    public override void UpdateState()
    {
        if (Animator != null && Animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {
            Animator.SetTrigger("Explosion");
        }
        Transform.Translate(Vector2.zero);

        if (CircleCollider.radius < explosionRadius)
        {
            CircleCollider.radius += Time.deltaTime * explosionSpeed;
        }
        else
        {
            Boom.gameObject.SetActive(false);
        }
    }
}
