using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShroom : AshPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.IceShroom;
        type = PlantType.Normal;
        attackPoint = 20; attackMode = 0;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
    }

    protected override void IdleUpdate()
    {
        if (canAct()) setAshState(AshPlantState.Explode); // 可行动即爆炸
    }

    protected override void Explode()
    {
        ExplodeEffect();
        List<Zombie> zombieList = ZombieManager.Instance.getZombieList();
        foreach (Zombie zombie in zombieList)
        {
            if (zombie)
            {
                if (zombie.armor2) zombie.armor2.UnderAttack(attackPoint); // 对二类防具也有伤害
                zombie.UnderAttack(attackPoint, dieMode);
                zombie.setFrozen();
            } 
        } 
        kill();
    }
}
