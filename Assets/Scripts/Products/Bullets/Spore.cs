using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : Bullet
{
    protected override void Awake()
    {
        base.Awake();
        id = BulletID.Spore;
        bulletType = BulletType.Shoot;
        bulletDirection = BulletDirection.Right;
        igniteID = BulletID.None;
    }
}
