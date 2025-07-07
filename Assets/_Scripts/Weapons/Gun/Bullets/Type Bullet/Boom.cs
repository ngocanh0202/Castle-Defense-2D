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
    protected override void SetInitPooler()
    {
        poolName = KeyOfObjPooler.Boom.ToString();
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
            explosionState = new BoomExplosionState(explosion, animator, transform , this);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BaseBullet"))
        {
            isBulletTime = false;
            collision.gameObject.SetActive(false);
            SetBoomState(explosionState);
        }
    }
}
