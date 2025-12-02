using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingRedWallNut : BowlingPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.BowlingRedWallNut;
        type = PlantType.Normal;
    }
}
