using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowyPea : Pea
{
    private float speedRatio;

    protected override void Awake()
    {
        base.Awake();
        id = BulletID.SnowyPea;
        igniteID = BulletID.Pea;
        speedRatio = 0.5f;
    }

    protected override void AttackZombie()
    {
        base.AttackZombie();
        if (targetZombie && CanAttack(targetZombie) && targetZombie.speedRatio >= speedRatio) targetZombie.setSpeedRatio(speedRatio);
    }
}
