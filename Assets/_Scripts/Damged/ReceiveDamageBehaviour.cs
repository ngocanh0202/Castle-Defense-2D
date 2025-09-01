using Common2D.CreateGameObject2D;
using UnityEngine;
using UnityEngine.UI;

public abstract class ReceiveDamageBehaviour : MonoBehaviour, IReceiveDamage
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    
    [Header("Damage Settings")]
    [SerializeField] private float damageInterval = 0.1f;
    [SerializeField] private bool isInvulnerable = false;
    
    [Header("UI References")]
    [SerializeField] protected Slider healthBar;
    
    public float CurrentHealth { get => currentHealth; set => currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float DamageInterval { get => damageInterval; set => damageInterval = value; }
    public bool IsAlive => CurrentHealth > 0;
    public bool IsInvulnerable { get => isInvulnerable; set => isInvulnerable = value; }
    private float lastDamageTime;

    protected virtual void Awake()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;
    }

    protected void InitializeComponents()
    {
        if (healthBar == null)
            healthBar = GetComponentInChildren<Slider>();
    }

    protected virtual void Start()
    {
        InitializeComponents();

        healthBar.maxValue = MaxHealth;
        healthBar.value = CurrentHealth;
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

        CurrentHealth -= damage;

        CreateGameObject.CreateTextPopup("-" + damage.ToString(),
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
    }
}