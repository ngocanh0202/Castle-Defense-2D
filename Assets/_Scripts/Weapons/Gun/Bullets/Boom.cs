using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : Bullet
{
    [Header("Boom Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D boomRigidbody2D;
    [Header("Boom States")]
    [SerializeField] Vector3 originLocalScale;
    public float timeToExplode = 1.5f;
    public float explosionRange = 2f;
    [SerializeField] bool isExploded = false;
    [SerializeField] float explodedTimer = 0f;

    protected override void SetInitPooler()
    {
        // todo: remove line 19 and 20 and 21
        timeToExplode = 0.75f;
        explosionRange = 4f;
        poolName = KeyGuns.Boom.ToString();
    }

    void Awake()
    {
        originLocalScale = transform.localScale;
    }

    override protected void Start()
    {
        base.Start();
        ResetState();
    }
    override protected void Update()
    {
        HandleBoomShooted();
   
    }
    override public void OnEnable()
    {
        base.OnEnable();
        InitializeComponents();
        ResetState();
    }
    void InitializeComponents()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (boomRigidbody2D == null)
            boomRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void HandleBoomShooted()
    {
        if (!isExploded)
            base.Update();
        else
        {
            if (animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
            {
                animator.SetTrigger("Explosion");
            }
            transform.Translate(Vector2.zero);

            if (explodedTimer < timeToExplode)
            {
                explodedTimer += Time.deltaTime;
                float newRadius = transform.localScale.x + Time.deltaTime * explosionRange;
                transform.localScale = new Vector3(newRadius, newRadius, 1);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void ResetState()
    {
        isExploded = false;
        transform.localScale = originLocalScale;
        explodedTimer = 0f;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        IReceiveDamage receiveDamage = collision.GetComponent<IReceiveDamage>();
        if (isExploded && receiveDamage != null)
        {
            SendDamage(incommingDamage, receiveDamage);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (shooterTransform != null && collider?.gameObject == shooterTransform.gameObject)
            return;
        if (!isExploded)
        {
            IReceiveDamage receiveDamage = collider.GetComponent<IReceiveDamage>();
            if (collider.CompareTag("BaseBullet"))
            {
                isBulletTime = false;
                collider.gameObject.SetActive(false);
                isExploded = true;
                explodedTimer = 0f;
            }
            else if (receiveDamage != null && !shooterTransform.CompareTag(collider.tag))
            {
                isBulletTime = false;
                isExploded = true;
                explodedTimer = 0f;
            }
        }
    }
}
