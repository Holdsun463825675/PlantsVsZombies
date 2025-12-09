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
        if (targetZombie && CanAttack(targetZombie)) // 解除减速和冰冻
        {
            targetZombie.relieveFrozen();
            targetZombie.relieveDeceleration();
            
        } 
    }

    protected override void Sputter()
    {
        base.Sputter();
        foreach (Zombie zombie in sputterTargetZombie) if (zombie && CanSputter(zombie)) // 解除减速和冰冻
        {
            zombie.relieveFrozen();
            zombie.relieveDeceleration();
        }
    }
}
