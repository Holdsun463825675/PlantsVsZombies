using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallNut : DefensePlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.TallNut;
        type = PlantType.Normal;
        maxHealth = 8000; currHealth = maxHealth;
    }
}
