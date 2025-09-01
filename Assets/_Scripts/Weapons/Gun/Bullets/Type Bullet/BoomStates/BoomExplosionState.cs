using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomExplosionState : BoomBaseState
{
    float explosionRange;
    float timeToExplode;
    float timer;
    Vector3 transformScale;

    public BoomExplosionState(CircleCollider2D circleCollider, Animator animator, Transform transform, Boom boom, float explosionRange) : base(circleCollider, animator, transform, boom)
    {
        transformScale = transform.localScale;
        this.explosionRange = explosionRange;
        timeToExplode = boom.timeToExplode;
    }

    public override void EnterState()
    {
        timer = 0f;
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
        if (Animator != null && Animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
        {
            Animator.SetTrigger("Explosion");
        }
        Transform.Translate(Vector2.zero);

        if (timer < timeToExplode)
        {
            timer += Time.deltaTime;
            float newRadius = Transform.localScale.x + Time.deltaTime * explosionRange;
            // CircleCollider.radius = newRadius;
            Transform.localScale = new Vector3(newRadius, newRadius, 1);
        }
        else
        {
            Boom.gameObject.SetActive(false);
        }
    }
}
