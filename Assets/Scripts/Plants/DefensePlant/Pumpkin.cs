using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : DefensePlant
{
    protected override void Awake()
    {
        base.Awake();
        type = PlantType.Surrounding;
    }
}
