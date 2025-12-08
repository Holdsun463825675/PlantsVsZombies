using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePea : Pea
{
    private float speedRatio;

    protected override void Awake()
    {
        base.Awake();
        id = BulletID.FirePea;
        igniteID = BulletID.None;
        speedRatio = 1.0f;
        attackPoint = 40;
        sputter = true;
        hitSound = BulletHitSound.FirePea;
        hitSoundPriority = 3;
    }

    protected override void AttackZombie()
    {
        base.AttackZombie();
        if (targetZombie && CanAttack(targetZombie)) targetZombie.relieveDeceleration(); // 解除减速
    }

    protected override void Sputter()
    {
        base.Sputter();
        foreach (Zombie zombie in sputterTargetZombie) if (zombie && CanSputter(zombie)) zombie.relieveDeceleration(); // 解除减速
    }
}
