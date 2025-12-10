using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSunflower : ProductionPlant
{
    protected SunID sunID;
    protected Sun sunPrefab;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.TwinSunflower;
        type = PlantType.Normal;
        prePlantID = PlantID.Sunflower;
        produceTimer = 17.0f + Random.Range(0.0f, 2.0f);
        sunID = SunID.Medium;
        sunPrefab = PrefabSystem.Instance.GetSunPrefab(sunID);
    }

    private void produceSun()
    {
        Sun sun = GameObject.Instantiate(sunPrefab, producePlace.position, Quaternion.identity);
        sun.setTargetY(transform.position.y);
        sun.setState(SunState.ProducedbySunflower);
    }

    protected override void Produce()
    {
        produceSun(); produceSun();
    }
}
