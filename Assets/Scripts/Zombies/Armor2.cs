using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor2 : MonoBehaviour
{
    public Zombie zombie;

    public ZombieUnderAttackSound underAttackSound = ZombieUnderAttackSound.Splat;
    public int underAttackSoundPriority = 1;

    private void Awake()
    {
        zombie = transform.parent.GetComponent<Zombie>();
    }

    public void UnderAttack(int point, int mode=0)
    {
        int hurtPoint = (int)((float)point * SettingSystem.Instance.settingsData.hurtRate); // 根据受伤比例计算伤害
        zombie.AddArmor2Health(-hurtPoint);
    }
}
