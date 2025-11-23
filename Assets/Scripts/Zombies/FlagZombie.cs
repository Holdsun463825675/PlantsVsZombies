using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.FlagZombie;
        baseSpeed = 0.1f;
        speed = Random.Range(0.5f, 0.6f);
        speedLevel = (speed - 0.5f) / baseSpeed;
    }
}
