using UnityEngine;

public interface IShootingStrategy
{
    string PoolNameString { get; set; }
    float GetCountDown();
    float SetCountDown(float value);
    int GetNumberOfBullets();
    int SetNumberOfBullets(int value);
    int GetMaxAmmo();
    int SetMaxAmmo(int value);
    float GetSpeedReloadAmmo();
    float SetSpeedReloadAmmo(float value);
    void Initialize(Transform bulletHolder);
    Bullet Shoot(Transform firePoint);
}