using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPad : Plant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.LilyPad;
        type = PlantType.Carrier;
        cellTypes = new List<CellType> { CellType.Pool };
    }
}
