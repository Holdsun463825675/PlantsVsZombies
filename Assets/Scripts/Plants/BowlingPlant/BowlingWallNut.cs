using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingWallNut : BowlingPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.BowlingWallNut;
        type = PlantType.Normal;
    }
}
