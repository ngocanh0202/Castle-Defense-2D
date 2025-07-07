using System;
using System.Collections;
using System.Collections.Generic;
using Common2D;
using Common2D.CreateGameObject2D;
using Common2D.Singleton;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] Transform bulletHolder;
    [SerializeField] bool canShoot = true ;
    [SerializeField] float countDown = 0.5f;
    [SerializeField] int maxAmmo;
    [SerializeField] int numberOfBullets;
    [SerializeField] float speedReloadAmmo;
    private IShootingStrategy currentBulletStrategy;
    private KeyOfObjPooler currentBulletStrategyType = KeyOfObjPooler.BaseBullet;
    private Coroutine attackCoroutine;
    private Coroutine reloadCoroutine;
    private Dictionary<KeyOfObjPooler, IShootingStrategy> bulletStrategies;
    public event Action<IShootingStrategy, string> OnSetBulletStrategy;
    public event Action<int, int> OnAmmoChanged;

    protected override void Start()
    {
        canShoot = true;
        InitializeBulletHolder();
        InitializeBulletStrategies();
        SetStrategy(KeyOfObjPooler.BaseBullet);
    }

    private void InitializeBulletHolder()
    {
        if (bulletHolder == null)
        {
            bulletHolder = GameObject.Find("=====PoolerHolder=====/Bullet")?.transform;
            if (bulletHolder == null)
            {
                Debug.LogError("Bullet holder not found!");
            }
        }
    }

    private void InitializeBulletStrategies()
    {
        bulletStrategies = new Dictionary<KeyOfObjPooler, IShootingStrategy>
        {
            { KeyOfObjPooler.BaseBullet, new BaseBulletStrategy()},
            { KeyOfObjPooler.RatlingGunBullet, new RatlingGunStrategy()},
            { KeyOfObjPooler.Boom, new BoomStrategy()}
        };

        foreach (var strategy in bulletStrategies.Values)
        {
            strategy.Initialize(bulletHolder);
        }
    }

    public void SetStrategy(KeyOfObjPooler newStrategyType)
    {
        if (bulletStrategies.ContainsKey(newStrategyType))
        {
            currentBulletStrategy = bulletStrategies[newStrategyType];
            countDown = currentBulletStrategy.GetCountDown();
            numberOfBullets = currentBulletStrategy.GetNumberOfBullets();
            maxAmmo = currentBulletStrategy.GetMaxAmmo();
            speedReloadAmmo = currentBulletStrategy.GetSpeedReloadAmmo();
            currentBulletStrategyType = newStrategyType;
            canShoot = true;
            OnSetBulletStrategy?.Invoke(
                currentBulletStrategy, currentBulletStrategyType.ToString()
            );
            OnAmmoChanged?.Invoke(numberOfBullets, maxAmmo);
        }
    }

    public override void Attack()
    {
        if (currentBulletStrategy != null && canShoot && numberOfBullets > 0)
        {
            currentBulletStrategy.Shoot(transform);
            numberOfBullets--;
            currentBulletStrategy.SetNumberOfBullets(numberOfBullets);
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

        // InputManager.Instance.DisableInput(
        //     InputType.ReloadAmmo,
        //     InputType.Attack,
        //     InputType.Move
        // );

        if (attackCoroutine != null)
        {
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
        // InputManager.Instance.EnableInput();
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(countDown);
        canShoot = true;
        StopAttack();
    }

    public void ChangeStrategy(KeyOfObjPooler newBulletStrategy)
    {
        if (newBulletStrategy == currentBulletStrategyType)
        {
            return;
        }
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        SetStrategy(newBulletStrategy);
    }
}
