using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowyPea : Pea
{
    private float speedRatio;

    protected override void Awake()
    {
        base.Awake();
        speedRatio = 0.5f;
    }

    protected override void AttackZombie()
    {
        base.AttackZombie();
        if (targetZombie.speedRatio >= speedRatio) targetZombie.setSpeedRatio(speedRatio);
    }
}
