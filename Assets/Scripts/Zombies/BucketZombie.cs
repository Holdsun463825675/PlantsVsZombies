using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.BucketZombie;
        spawnWeight = 4.0f;
        maxArmor1Health = 1100; currArmor1Health = maxArmor1Health;

        underAttackSound = ZombieUnderAttackSound.Shield;
        underAttackSoundPriority = 2;
    }
}
