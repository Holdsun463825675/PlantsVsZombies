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

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) targetRows = new List<int> { Mathf.Max(1, row - 1), row, Mathf.Min(row + 1, CellManager.Instance.maxRow) }; // 只能攻击本行、上一行、下一行
    }

    protected override void IdleUpdate()
    {
        setAshState(AshPlantState.Explode); // 种下即爆炸
    }

    protected override void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_bomb_cherrybomb);
        base.Explode();
    }
}
