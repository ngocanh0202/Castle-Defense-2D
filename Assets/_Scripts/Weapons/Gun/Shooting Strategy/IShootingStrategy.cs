using UnityEngine;

public interface IShootingStrategy
{
    Bullet Shoot(Transform firePoint, float plusDamage = 0f);
}