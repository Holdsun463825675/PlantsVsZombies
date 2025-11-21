using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.FlagZombie;
        base_x_Speed = 0.1f;
        x_Speed = -Random.Range(0.5f, 0.6f);
        speedLevel = (-x_Speed - 0.5f) / base_x_Speed;
    }
}
