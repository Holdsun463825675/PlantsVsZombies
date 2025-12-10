using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : ProductionPlant
{
    protected SunID sunID;
    protected Sun sunPrefab;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Sunflower;
        type = PlantType.Normal;
        produceTimer = 17.0f + Random.Range(0.0f, 2.0f);
        sunID = SunID.Medium;
        sunPrefab = PrefabSystem.Instance.GetSunPrefab(sunID);
    }

    protected override void Produce()
    {
        Sun sun = GameObject.Instantiate(sunPrefab, producePlace.position, Quaternion.identity);
        sun.setTargetY(transform.position.y);
        sun.setState(SunState.ProducedbySunflower);
    }
}
