using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.NormalZombie;
    }
}
