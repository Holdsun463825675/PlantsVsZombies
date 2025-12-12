using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypnoShroom : Plant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.HypnoShroom;
        type = PlantType.Normal;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
    }

    public override void UnderAttack(int point, int mode = 0, Zombie zombie = null)
    {
        if (!canAct()) base.UnderAttack(point, mode, zombie);
        else
        {
            if (zombie != null && mode == 0)
            {
                AudioManager.Instance.playClip(ResourceConfig.sound_plant_floop);
                zombie.setTemptation(); // ÷È»ó½©Ê¬
                kill();
            }
        }
    }
}
