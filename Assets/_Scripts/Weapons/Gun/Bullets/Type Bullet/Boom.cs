using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : Bullet
{
    [Header("Boom Components")]
    [SerializeField] private CircleCollider2D explosion;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D boomRigidbody2D;
    public BoomBaseState currentState;
    public BoomExplosionState explosionState;
    public BoomShootState shootState;
    [Header("Boom States")]
    public float timeToExplode = 1.5f;
    public float explosionRange = 2f;

    protected override void SetInitPooler()
    {
        // todo: remove line 19 and 20 and 21
        timeToExplode = 0.75f;
        explosionRange = 4f;
        poolName = KeyGuns.Boom.ToString();
    }

    override protected void Start()
    {
        base.Start();
        InitializeState();
    }
    override protected void Update()
    {
        currentState.UpdateState();
        base.Update();
    }
    override public void OnEnable()
    {
        base.OnEnable();
        InitializeComponents();
        InitializeState();
    }
    void InitializeComponents()
    {
        if (explosion == null)
            explosion = GetComponent<CircleCollider2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (boomRigidbody2D == null)
            boomRigidbody2D = GetComponent<Rigidbody2D>();
    }
    void InitializeState()
    {
        if (explosionState == null)
            explosionState = new BoomExplosionState(explosion, animator, transform , this, explosionRange);
        if (shootState == null)
            shootState = new BoomShootState(explosion, animator, transform , this, speed);

        if (currentState == explosionState)
        {
            currentState = shootState;
            currentState.EnterState();
        }
        else if (currentState == shootState)
        {
            currentState.EnterState();
        }
        else
        {
            currentState = shootState;
            currentState.EnterState();
        }
    }

    public void SetBoomState(BoomBaseState newBoomBaseState)
    {
        currentState = newBoomBaseState;
        currentState.EnterState();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        IReceiveDamage receiveDamage = collision.GetComponent<IReceiveDamage>();
        if (currentState == explosionState && receiveDamage != null)
        {
            SendDamage(damage, receiveDamage);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == shooterTransform?.gameObject)
            return;
        if (currentState == shootState)
        {
            IReceiveDamage receiveDamage = collider.GetComponent<IReceiveDamage>();
            if (collider.CompareTag("BaseBullet"))
            {
                isBulletTime = false;
                collider.gameObject.SetActive(false);
                SetBoomState(explosionState);
            }
            else if (receiveDamage != null && !shooterTransform.CompareTag(collider.tag))
            {
                isBulletTime = false;
                SetBoomState(explosionState);
            }
        }
    }
}
