using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDoorZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.ScreenDoorZombie;
        spawnWeight = 4.0f;
        maxArmor2Health = 1100; currArmor2Health = maxArmor2Health;
    }
}
