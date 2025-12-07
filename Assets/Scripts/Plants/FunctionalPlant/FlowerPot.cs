using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : Plant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.FlowerPot;
        type = PlantType.Carrier;
        cellTypes = new List<CellType> { CellType.Grass, CellType.Roof };
    }
}
