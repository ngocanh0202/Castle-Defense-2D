using System.Collections;
using System.Collections.Generic;
using Common2D;
using UnityEngine;

public class RatlingGunStrategy : CommonBulletStrategy
{
    public override string PoolNameString => KeyGuns.RatlingGunBullet.ToString();
}
