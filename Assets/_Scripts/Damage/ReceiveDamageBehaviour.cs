using System;
using Common2D.CreateGameObject2D;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReceiveDamageBehaviour : MonoBehaviour, IReceiveDamage
{
    [Header("Health Settings")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [Header("Damage Settings")]
    [SerializeField] private float damageInterval = 0.1f;
    [SerializeField] private bool isInvulnerable = false;

    [Header("Components")]
    [SerializeField] protected Slider healthBar;
    [SerializeField] protected ObjectStat objectStatus;

    public Action<Transform> OnDie;

    public float CurrentHealth { get => currentHealth; set => currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float DamageInterval { get => damageInterval; set => damageInterval = value; }
    public bool IsAlive => CurrentHealth > 0;
    public bool IsInvulnerable { get => isInvulnerable; set => isInvulnerable = value; }
    private float lastDamageTime;

    protected virtual void OnInitStat()
    {
        MaxHealth = objectStatus.GetStatValue(StatType.Vitality);
        CurrentHealth = MaxHealth;

        healthBar.maxValue = MaxHealth;
        healthBar.value = CurrentHealth;
    }

    protected virtual void Awake()
    {
        InitializeComponents();
        OnInitStat();
    }

    protected void InitializeComponents()
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<Slider>();
        if (objectStatus == null)
            objectStatus = GetComponent<ObjectStat>();
    }

    void OnEnable()
    {
        InitializeComponents();
        OnInitStat();
    }

    public virtual bool CanReceiveDamage()
    {
        return IsAlive &&
               !IsInvulnerable &&
               Time.time >= lastDamageTime + DamageInterval;
    }

    protected void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        healthBar.value = CurrentHealth;
    }

    public virtual void ReceiveDamage(float damage)
    {
        if (!CanReceiveDamage())
            return;

        damage = Mathf.Abs(damage);
        lastDamageTime = Time.time;

        float inCommingDamage = damage - objectStatus.GetStatValue(StatType.Defense);
        if (inCommingDamage < 0)
        {
            inCommingDamage = 0.1f;
        }
        
        CurrentHealth -= inCommingDamage;
        CreateGameObject.CreateTextPopup("-" + inCommingDamage.ToString(),
                healthBar.transform.position, Color.red);
        healthBar.value = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
        OnDie?.Invoke(this.transform);
    }
}