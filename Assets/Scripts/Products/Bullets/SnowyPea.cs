using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowyPea : Pea
{
    protected override void Awake()
    {
        base.Awake();
        id = BulletID.SnowyPea;
        igniteID = BulletID.Pea;
    }

    protected override void AttackZombie()
    {
        base.AttackZombie();
        //if (targetZombie && CanAttack(targetZombie)) targetZombie.setDeceleration();
        if (targetZombie && CanAttack(targetZombie)) targetZombie.setFrozen();
    }
}
