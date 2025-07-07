using System.Collections;
using System.Collections.Generic;
using Common2D;
using UnityEngine;

public class RatlingGunStrategy : IShootingStrategy
{
    public RatlingGun bulletPrefab;
    public GunScriptableObject gunScriptableObject;
    public BulletState bulletState;
    private float countDown;
    private int numberOfBullets;
    private int maxAmmo;
    private float speedReloadAmmo;
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

    public void Initialize(Transform bulletHolder)
    {
        bulletPrefab = ResourcesManager.GetBulletPrefab(KeyOfObjPooler.RatlingGunBullet.ToString()).GetComponent<RatlingGun>();
        if (bulletPrefab == null)
        {
            Debug.LogError("RatlingGun bullet prefab not found!");
            return;
        }
        gunScriptableObject = ResourcesManager.GetGunScriptableObject(KeyOfObjPooler.RatlingGunBullet.ToString());
        if (gunScriptableObject == null)
        {
            Debug.LogError("GunScriptableObject for RatlingGunBullet not found!");
            return;
        }
        countDown = gunScriptableObject.countDown;
        numberOfBullets = gunScriptableObject.numberOfBullets;
        maxAmmo = gunScriptableObject.maxAmmo;
        speedReloadAmmo = gunScriptableObject.speedReloadAmmo;
        bulletState = gunScriptableObject.bulletState;
        bulletPrefab.SetBulletState(bulletState.speed, bulletState.damage, bulletState.lifeTime);

        ObjectPooler.InitObjectPooler<RatlingGun>(KeyOfObjPooler.RatlingGunBullet.ToString(), 1, bulletPrefab, (newPrefab) =>
        {
            newPrefab.transform.SetParent(bulletHolder);
        });
    }

    public Bullet Shoot(Transform firePoint)
    {
        var bullet = ObjectPooler.GetObject<RatlingGun>(KeyOfObjPooler.RatlingGunBullet.ToString());
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            return bullet;
        }
        return null;
    }
}
