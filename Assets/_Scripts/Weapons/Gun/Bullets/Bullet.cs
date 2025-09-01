using System;
using Common2D;
using UnityEngine;

public abstract class Bullet : CommonPoolObject, ISendDamage
{
    [Header("Bullet Settings")]
    [SerializeField] public string poolName;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] public float timer = 0f;
    [SerializeField] protected bool isBulletTime = true;
    [Header("Shooter")]
    [SerializeField] protected Transform shooterTransform;
    public override string PoolName { get => poolName; set => poolName = value; }    
    protected abstract void SetInitPooler();

    virtual protected void Start()
    {
        InitValue();
    }

    virtual protected void Update()
    {
        BulletTimeLife();
    }

    public virtual void OnEnable()
    {
        timer = 0f;
        isBulletTime = true;
        InitValue();
    }

    void InitValue()
    {
        SetInitPooler();
        if (string.IsNullOrEmpty(poolName))
        {
            Debug.LogError("Pool name is not set for the bullet.");
        }
    }

    public void SetBulletState(float speed, float damage, float lifeTime)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
    }

    public void SetShooterTransform(Transform shooterTransform)
    {
        this.shooterTransform = shooterTransform;
    }

    private void BulletTimeLife()
    {
        if (!isBulletTime)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
            timer = 0f;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
          if (collision.gameObject == shooterTransform?.gameObject)
            return;
        IReceiveDamage receiveDamage = collision.GetComponent<IReceiveDamage>();
        if (receiveDamage != null && !shooterTransform.CompareTag(collision.tag))
        {
            SendDamage(damage, receiveDamage);
            gameObject.SetActive(false);
        }
    }


    public void SendDamage(float damage, IReceiveDamage target)
    {
        target.ReceiveDamage(damage);
    }
}
