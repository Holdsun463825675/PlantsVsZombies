using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowyPea : Pea
{
    private float speedRatio; // ¼õËÙ±ÈÀý

    protected override void Awake()
    {
        base.Awake();
        speedRatio = 0.5f;
    }

    protected override void Attack()
    {
        base.Attack();
        if (target.speedRatio >= speedRatio) target.setSpeedRatio(speedRatio);
    }
}
