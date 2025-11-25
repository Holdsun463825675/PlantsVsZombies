using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : AshPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.CherryBomb;
        type = PlantType.Normal;
    }

    protected override void IdleUpdate()
    {
        setAshState(AshPlantState.Explode); // ÷÷œ¬º¥±¨’®
    }

    protected override void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_bomb_cherrybomb);
        base.Explode();
    }
}
