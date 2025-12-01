using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor2 : MonoBehaviour
{
    public Zombie zombie;

    public ZombieUnderAttackSound underAttackSound = ZombieUnderAttackSound.Splat;
    public int underAttackSoundPriority = 1;

    public void UnderAttack(int point, int mode=0)
    {
        zombie.AddArmor2Health(-point);
    }
}
