using System;
using UnityEngine;

public class Tower
{
    public TowerDefenseBehaviour prefab;
    public TowerStat Stat;

    public Tower(TowerDefenseBehaviour prefab)
    {
        this.prefab = prefab;
    }

    public void SetStat(TowerStat Stat)
    {
        this.Stat = Stat;
    }
}

[RequireComponent(typeof(TowerStat))]
public class TowerDefenseBehaviour : CommonPoolObject
{
    public Action<TowerDefenseBehaviour> onTowerDisable;
    public Action<TowerDefenseBehaviour> onTowerEnable;
    [Header("Tower Components")]
    [SerializeField] TowerStat towerStat;
    [SerializeField] Transform gunTransform;
    [SerializeField] Transform target;
    [SerializeField] Gun gun;
    [Header("Tower Settings")]
    [SerializeField] public bool isStopAttack;
    [SerializeField] public KeyGuns keyGunType = KeyGuns.None;
    [Header("Attack Settings")]
    [SerializeField] bool isConfirmingAttack;
    [Header("State Check Settings")]
    [SerializeField] float checkDelay = 0.33f; 
    [SerializeField] float lastCheckTime;

    void Start()
    {
        if (gun == null)
        {
            gun = GetComponentInChildren<Gun>();
            gunTransform = gun.transform;
            if (keyGunType == KeyGuns.None)
                Debug.LogWarning($"TowerDefenseBehaviour {gameObject.name}: KeyGunType is None, please set a valid KeyGunType.");
            else
                gun?.SetStrategy(keyGunType);
        }

        if (towerStat == null)
            towerStat = GetComponent<TowerStat>();

        target = null;
        isStopAttack = false;
        lastCheckTime = 0f;
    }

    void Update()
    {
        if(isStopAttack) return;
        if (Time.time - lastCheckTime >= checkDelay)
        {
            CheckTargetInRange();
            lastCheckTime = Time.time;
        }
        HandleAttack();
    }

    void CheckTargetInRange()
    {
        if (IsEnemyInRange(out Collider2D enemyCollider))
        {
            target = enemyCollider.transform;
            isConfirmingAttack = true;
        }
        else
        {
            target = null;
            isConfirmingAttack = false;
        }
    }

    void HandleAttack()
    {
        if (isConfirmingAttack && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            gunTransform.right = direction;
            gun.Attack(false);
        }

    }

    bool IsEnemyInRange(out Collider2D enemyCollider)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, towerStat.GetStatValue<float>(TowerStatType.Range));
        foreach (var collider in hitColliders)
        {
            IReceiveDamage enemyReceiveDamage = collider.GetComponent<IReceiveDamage>();
            if (enemyReceiveDamage != null && !collider.gameObject.CompareTag(gameObject.tag))
            {
                enemyCollider = collider;
                return true;
            }
        }
        enemyCollider = null;
        return false;
    }

    void OnDrawGizmos()
    {
        if (isStopAttack) return;
        if (isConfirmingAttack)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(transform.position, towerStat.GetStatValue<float>(TowerStatType.Range));
        if(target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    void OnEnable()
    {
        onTowerEnable?.Invoke(this);
    }

    protected override void OnDisable()
    {
        onTowerDisable?.Invoke(this);
        base.OnDisable();
    }
}