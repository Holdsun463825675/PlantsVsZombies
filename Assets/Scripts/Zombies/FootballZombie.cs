using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballZombie : Zombie
{
    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.FootballZombie;
        spawnWeight = 6.0f;
        setMoveSpeed(0.5f, 0.6f);
        maxArmor1Health = 1400; currArmor1Health = maxArmor1Health;

        underAttackSound = ZombieUnderAttackSound.Plastic;
        underAttackSoundPriority = 1;
    }
}
