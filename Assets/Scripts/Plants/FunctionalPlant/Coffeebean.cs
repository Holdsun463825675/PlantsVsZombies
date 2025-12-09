using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Coffeebean : Plant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Coffeebean;
        type = PlantType.Flight;
        cellTypes = new List<CellType> { CellType.None };
    }

    protected override void IdleUpdate()
    {
        if (canAct()) anim.SetTrigger(AnimatorConfig.plant_effect);
    }

    private void AwakeAllPlants()
    {
        if (!cell) return;
        foreach (KeyValuePair<PlantType, List<Plant>> pair in cell.plants)
        {
            foreach (Plant pl in pair.Value) if (pl) pl.awakeFromSleep();
        }
        kill();
    }
}
