using System;
using Common2D;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IPooler
{
    public event EventHandler<PoolerEventArgs> OnSetInactive;
    [Header("Bullet Settings")]
    [SerializeField] public string poolName;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float lifeTime = 5f;
    [SerializeField] public float timer = 0f;
    [SerializeField] protected bool isBulletTime = true;

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
    void OnDisable()
    {
        OnSetInactive?.Invoke(this, new PoolerEventArgs { key = poolName });
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
}
