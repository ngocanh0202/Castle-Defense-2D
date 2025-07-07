using Common2D;
using UnityEngine;

public class BoomStrategy : IShootingStrategy
{
    public Boom bulletPrefab;
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
        bulletPrefab = ResourcesManager.GetBulletPrefab(KeyOfObjPooler.Boom.ToString()).GetComponent<Boom>();
        if (bulletPrefab == null)
        {
            Debug.LogError("Boom bullet prefab not found!");
            return;
        }
        gunScriptableObject = ResourcesManager.GetGunScriptableObject(KeyOfObjPooler.Boom.ToString());
        if (gunScriptableObject == null)
        {
            Debug.LogError("GunScriptableObject for Boom not found!");
            return;
        }
        bulletState = gunScriptableObject.bulletState;
        countDown = gunScriptableObject.countDown;
        numberOfBullets = gunScriptableObject.numberOfBullets;
        maxAmmo = gunScriptableObject.maxAmmo;
        speedReloadAmmo = gunScriptableObject.speedReloadAmmo;
        bulletPrefab.SetBulletState(bulletState.speed, bulletState.damage, bulletState.lifeTime);
        ObjectPooler.InitObjectPooler<Boom>(KeyOfObjPooler.Boom.ToString(), 1, bulletPrefab, (newPrefab) =>
        {
            newPrefab.transform.SetParent(bulletHolder);
        });
    }

    public Bullet Shoot(Transform firePoint)
    {
        var bullet = ObjectPooler.GetObject<Boom>(KeyOfObjPooler.Boom.ToString());
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            return bullet;
        }
        return null;
    }
}
