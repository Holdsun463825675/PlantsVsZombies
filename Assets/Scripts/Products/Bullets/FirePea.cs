using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePea : Pea
{
    private float speedRatio;

    protected override void Awake()
    {
        base.Awake();
        speedRatio = 1.0f;
        attackPoint = 40;
        sputter = true;
        hitSound = BulletHitSound.FirePea;
        hitSoundPriority = 3;
    }

    protected override void AttackZombie()
    {
        base.AttackZombie();
        if (targetZombie.speedRatio < speedRatio) targetZombie.setSpeedRatio(speedRatio); // 解除减速
    }

    protected override void Sputter()
    {
        base.Sputter();
        foreach (Zombie zombie in sputterTargetZombie) if (zombie.speedRatio < speedRatio) zombie.setSpeedRatio(speedRatio); // 解除减速
    }
}
