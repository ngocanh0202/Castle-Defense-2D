using System;
using UnityEngine;

public class BaseBullet : Bullet
{
    protected override void SetInitPooler()
    {
        poolName = KeyGuns.BaseBullet.ToString();
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
