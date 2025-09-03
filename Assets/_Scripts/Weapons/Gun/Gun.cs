using System;
using System.Collections;
using System.Collections.Generic;
using Common2D.Singleton;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] public bool canShoot = true;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public float countDown = 0.5f;
    [SerializeField] public int maxAmmo;
    [SerializeField] public int numberOfBullets;
    [SerializeField] public float speedReloadAmmo;
    [Header("Components")]
    [SerializeField] private ObjectStat ObjectStatus;
    private CommonBulletStrategy currentBulletStrategy;
    private KeyGuns currentBulletStrategyType;
    private Coroutine attackCoroutine;
    private Coroutine reloadCoroutine;
    private Dictionary<KeyGuns, CommonBulletStrategy> bulletStrategies;
    public event Action<CommonBulletStrategy, string> OnSetBulletStrategy;
    public event Action<int, int> OnAmmoChanged;

    private void Awake()
    {
        currentBulletStrategyType = KeyGuns.None;
        canShoot = true;
        isAttacking = false;
        if(ObjectStatus == null)
            ObjectStatus = GetComponentInParent<ObjectStat>();
        
        InitializeBulletStrategies();
    }

    private void InitializeBulletStrategies()
    {
        bulletStrategies = new Dictionary<KeyGuns, CommonBulletStrategy>
        {
            { KeyGuns.BaseBullet, new BaseBulletStrategy()},
            { KeyGuns.RatlingGunBullet, new RatlingGunStrategy()},
            { KeyGuns.Boom, new BoomStrategy()}
        };
    }

    public void SetStrategy(KeyGuns newStrategyType)
    {
        if (bulletStrategies.ContainsKey(newStrategyType) && newStrategyType != currentBulletStrategyType)
        {
            currentBulletStrategy = bulletStrategies[newStrategyType];
            countDown = currentBulletStrategy.GetCountDown();
            numberOfBullets = currentBulletStrategy.GetNumberOfBullets();
            maxAmmo = currentBulletStrategy.GetMaxAmmo();
            speedReloadAmmo = currentBulletStrategy.GetSpeedReloadAmmo();
            currentBulletStrategyType = newStrategyType;
            canShoot = true;
            isAttacking = false;
            OnSetBulletStrategy?.Invoke(
                currentBulletStrategy, currentBulletStrategyType.ToString()
            );
            OnAmmoChanged?.Invoke(numberOfBullets, maxAmmo);
        }
    }

    public void Attack(bool isPlayer = true)
    {
        if (currentBulletStrategy != null && canShoot && numberOfBullets > 0 && gameObject.activeInHierarchy)
        {
            currentBulletStrategy.Shoot(transform, ObjectStatus.GetStatValue(StatType.Attack));
            if (isPlayer)
            {
                numberOfBullets--;
                currentBulletStrategy.SetNumberOfBullets(numberOfBullets);
            }
            canShoot = false;
            OnAmmoChanged?.Invoke(numberOfBullets, maxAmmo);
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }
    }

    public void ReloadAmmo(int ammo, Action<int> onReturnAmmoRedundant = null)
    {
        if (ammo <= 0 || numberOfBullets >= maxAmmo)
        {
            NotificationSystem.Instance.ShowNotification(
                "No ammo to reload or already full!",
                NotificationType.Warning
            );
            return;
        }
        if (attackCoroutine != null)
        {
            isAttacking = false;
            StopCoroutine(attackCoroutine);
        }
        if (reloadCoroutine == null)
        {
            canShoot = false;
            reloadCoroutine = StartCoroutine(ReloadAmmoCoroutine(ammo, onReturnAmmoRedundant));
        }
    }
    IEnumerator ReloadAmmoCoroutine(int ammo, Action<int> onReturnAmmoRedundant = null)
    {
        int ammoToReload = Mathf.Min(ammo, maxAmmo - numberOfBullets);
        if (ammoToReload > 0)
        {
            for (int i = 0; i < ammoToReload; i++)
            {
                yield return new WaitForSeconds(speedReloadAmmo);
                numberOfBullets++;
                currentBulletStrategy.SetNumberOfBullets(numberOfBullets);
                OnAmmoChanged?.Invoke(numberOfBullets, maxAmmo);
            }
        }
        if (onReturnAmmoRedundant != null)
        {
            onReturnAmmoRedundant.Invoke(ammo - ammoToReload);
        }
        reloadCoroutine = null;
        canShoot = true;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(countDown);
        canShoot = true;
        StopAttack();
    }

    protected void StopAttack()
    {
        isAttacking = false;
    }

    void OnEnable()
    {
        isAttacking = false;
        canShoot = true;
    }

    void OnDisable()
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}
