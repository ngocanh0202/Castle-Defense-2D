using System;
using Common2D;
using UnityEngine;

public abstract class Bullet : CommonPoolObject, ISendDamage
{
    [Header("Bullet Settings")]
    [SerializeField] public string poolName;
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float plusDamage = 0f;
    [SerializeField] public float incommingDamage { get => damage + plusDamage; set => throw new NotImplementedException();}
    [SerializeField] public float lifeTime = 5f;
    [SerializeField] public float timer = 0f;
    [SerializeField] public bool isBulletTime = true;
    [Header("Shooter")]
    [SerializeField] public Transform shooterTransform;
    public override string PoolName { get => poolName; set => poolName = value; }    
    protected abstract void SetInitPooler();

    virtual protected void Start()
    {
        InitValue();
    }

    virtual protected void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
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

    public void SetShooterStat(Transform shooterTransform, float plusDamage)
    {
        this.shooterTransform = shooterTransform;
        this.plusDamage = plusDamage;
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
        if (shooterTransform != null && collision?.gameObject == shooterTransform.gameObject)
            return;

        IReceiveDamage receiveDamage = collision.GetComponent<IReceiveDamage>();
        if (receiveDamage != null)
        {
            if (shooterTransform == null || !shooterTransform.CompareTag(collision.tag))
            {
                SendDamage(incommingDamage, receiveDamage);
                gameObject.SetActive(false);
            }
        }
    }


    public void SendDamage(float damage, IReceiveDamage target)
    {
        target.ReceiveDamage(damage);
    }
}
