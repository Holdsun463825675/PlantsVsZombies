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
        baseSpeed = 0.1f;
        speed = Random.Range(0.5f, 0.6f);
        speedLevel = (speed - 0.5f) / baseSpeed;
        maxArmor1Health = 1400; currArmor1Health = maxArmor1Health;

        underAttackSound = ZombieUnderAttackSound.Plastic;
        underAttackSoundPriority = 1;
    }

    protected override void Update()
    {
        base.Update();
        if (currArmor1Health == 0.0f) underAttackSound = ZombieUnderAttackSound.Splat;
    }
}
