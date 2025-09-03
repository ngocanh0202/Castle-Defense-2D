using Common2D;
using UnityEngine;

public class CommonBulletStrategy : IShootingStrategy
{
    public Bullet bulletPrefab;
    public GunScriptableObject gunScriptableObject;
    public BulletState bulletState;
    private float countDown;
    private int numberOfBullets;
    private int maxAmmo;
    private float speedReloadAmmo;
    public virtual string PoolNameString { get; set; }

    public CommonBulletStrategy()
    {
        bulletPrefab = ResourcesManager.GetBulletPrefab(PoolNameString).GetComponent<Bullet>();
        if (bulletPrefab == null)
        {
            Debug.LogError("BaseBullet prefab not found!"+ PoolNameString);
            return;
        }
        gunScriptableObject = ResourcesManager.GetGunScriptableObject(PoolNameString);
        if (gunScriptableObject == null)
        {
            Debug.LogError("GunScriptableObject for BaseBullet not found!");
            return;
        }
        bulletState = gunScriptableObject.bulletState;
        countDown = gunScriptableObject.countDown;
        numberOfBullets = gunScriptableObject.numberOfBullets;
        maxAmmo = gunScriptableObject.maxAmmo;
        speedReloadAmmo = gunScriptableObject.speedReloadAmmo;
        bulletPrefab.SetBulletState(bulletState.speed, bulletState.damage, bulletState.lifeTime);
        if (ObjectPooler.Instance.IsObjectPoolerExist(PoolNameString))
        {
            return;
        }
        ObjectPooler.Instance.InitObjectPooler<Bullet>(PoolNameString, 3, bulletPrefab);
    }

    public float GetCountDown()
    {
        return countDown;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public int GetNumberOfBullets()
    {
        return numberOfBullets;
    }

    public float SetCountDown(float value)
    {
        countDown = value;
        return countDown;
    }

    public int SetMaxAmmo(int value)
    {
        maxAmmo = value;
        return maxAmmo;
    }

    public int SetNumberOfBullets(int value)
    {
        numberOfBullets = value;
        return numberOfBullets;
    }

    public float GetSpeedReloadAmmo()
    {
        return speedReloadAmmo;
    }

    public float SetSpeedReloadAmmo(float value)
    {
        speedReloadAmmo = value;
        return speedReloadAmmo;
    }

    public Bullet Shoot(Transform firePoint, float plusDamage = 0f)
    {
        var bullet = ObjectPooler.Instance.GetObject<Bullet>(PoolNameString);
        if (bullet != null)
        {
            bullet.SetShooterStat(firePoint.transform.parent, plusDamage);
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            return bullet;
        }
        return null;
    }
}
