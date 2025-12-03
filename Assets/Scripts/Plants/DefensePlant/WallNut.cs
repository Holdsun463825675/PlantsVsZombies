using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : DefensePlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.WallNut;
        type = PlantType.Normal;
        maxHealth = 4000; currHealth = maxHealth;
    }
}
