using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomShroom : AshPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.DoomShroom;
        type = PlantType.Normal;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
    }

    protected override void IdleUpdate()
    {
        if (canAct()) setAshState(AshPlantState.Explode); // 可行动即爆炸
    }

    protected override void ExplodeEffect()
    {
        if (!explodeEffect) return;
        GameObject.Instantiate(explodeEffect, cell.transform.position, Quaternion.identity); // 在格子位置生成特效
    }

    protected override void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_bomb_doomshroom);
        Cell c = cell;
        base.Explode();
        c.setCellProp(CellProp.Crater, true); // 留下弹坑
    }
}
