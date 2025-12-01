using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pea : Bullet
{
    protected override void Awake()
    {
        base.Awake();
        id = BulletID.Pea;
        igniteID = BulletID.FirePea;
    }
}
