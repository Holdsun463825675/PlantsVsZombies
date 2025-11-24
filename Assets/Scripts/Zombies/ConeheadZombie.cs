using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeheadZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.ConeHeadZombie;
        spawnWeight = 2.0f;
        maxArmor1Health = 370; currArmor1Health = maxArmor1Health;

        underAttackSound = ZombieUnderAttackSound.Plastic;
        underAttackSoundPriority = 1;
    }

    protected override void Update()
    {
        base.Update();
        if (currArmor1Health == 0.0f) underAttackSound = ZombieUnderAttackSound.Splat;
    }
}
