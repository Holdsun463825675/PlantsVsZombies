using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumeShroom : AttackPlant
{
    protected int attackPoint;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.FumeShroom;
        type = PlantType.Normal;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        attackPoint = 20;
    }

    protected override void Attack()
    {
        if (attackEffect && effectPlace) // 生成攻击特效
        {
            currAttackEffect = GameObject.Instantiate(attackEffect, effectPlace.position, Quaternion.identity);
        }
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_fume);
        
        List<Armor2> targetArmor2s = new List<Armor2>(targetArmor2);
        List<Zombie> targetZombies = new List<Zombie>(targetZombie);
        bool clip = false;
        foreach (Zombie zombie in targetZombies)
        {
            if (zombie && CanAttack(zombie))
            {
                zombie.UnderAttack(attackPoint);
                clip = true;
            }
        }
        foreach (Armor2 armor2 in targetArmor2s) if (armor2 && CanAttack(armor2)) armor2.UnderAttack(attackPoint);

        if (clip) AudioManager.Instance.playHitClip(BulletHitSound.None, 0, ZombieUnderAttackSound.Splat, 1);
    }
}
