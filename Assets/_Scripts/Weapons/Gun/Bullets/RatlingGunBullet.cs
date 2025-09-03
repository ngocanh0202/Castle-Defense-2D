using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatlingGun : Bullet
{
    protected override void SetInitPooler()
    {
        poolName = KeyGuns.RatlingGunBullet.ToString();
    }
    override protected void Start()
    {
        base.Start();
    }
    override protected void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        base.Update();
    }
}
