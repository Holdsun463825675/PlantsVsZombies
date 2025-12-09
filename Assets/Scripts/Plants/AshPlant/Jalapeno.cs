using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : AshPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Jalapeno;
        type = PlantType.Normal;
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) targetRows = new List<int> { row }; // 只能攻击本行
    }

    protected override void IdleUpdate()
    {
        if (canAct()) setAshState(AshPlantState.Explode); // 可行动即爆炸
    }

    protected override void ExplodeEffect()
    {
        if (!explodeEffect) return;
        float speedRatio = 0.5f;
        float cellWidth = Mathf.Abs(CellManager.Instance.getCell(row, 1).transform.position.x - CellManager.Instance.getCell(row, 2).transform.position.x);
        List<Vector3> leftPlaces = new List<Vector3>(), rightPlaces = new List<Vector3>();
        // 每格生成一个火焰
        foreach (Cell cell in CellManager.Instance.cellList)
        {
            if (targetRows.Contains(cell.row))
            {
                if (cell.col == CellManager.Instance.minCol) leftPlaces.Add(new Vector3(cell.transform.position.x - cellWidth, cell.transform.position.y, cell.transform.position.z));
                else if (cell.col == CellManager.Instance.maxCol) rightPlaces.Add(new Vector3(cell.transform.position.x + cellWidth, cell.transform.position.y, cell.transform.position.z));
                Animator anim = GameObject.Instantiate(explodeEffect, cell.transform.position, Quaternion.identity).GetComponent<Animator>();
                anim.speed = (2.0f - (float)cell.col / (float)(CellManager.Instance.maxCol + 1)) * speedRatio;
            }
        }
        // 最左和最右也需要生成火焰
        foreach (Vector3 place in leftPlaces)
        {
            Animator anim = GameObject.Instantiate(explodeEffect, place, Quaternion.identity).GetComponent<Animator>();
            anim.speed = 2.0f * speedRatio;
        }
        foreach (Vector3 place in rightPlaces)
        {
            Animator anim = GameObject.Instantiate(explodeEffect, place, Quaternion.identity).GetComponent<Animator>();
            anim.speed = 1.0f * speedRatio;
        }
    }

    protected override void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_bomb_jalapeno);
        // 消除冰道
        foreach (Cell cell in CellManager.Instance.cellList) if (targetRows.Contains(cell.row)) cell.setCellProp(CellProp.IceTunnel, false);
        // 解除冰冻与减速
        List<Zombie> targetZombies = new List<Zombie>(targetZombie);
        foreach (Zombie zombie in targetZombies)
        {
            if (CanAttack(zombie))
            {
                zombie.relieveFrozen();
                zombie.relieveDeceleration();
            }
        }
        base.Explode();
    }
}
