using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : ProductionPlant
{
    public Sun sunPrefab;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Sunflower;
        type = PlantType.Normal;
        produceTimer = 17.0f + Random.Range(0.0f, 2.0f);
    }

    protected override void Produce()
    {
        Sun sun = GameObject.Instantiate(sunPrefab, transform.position, Quaternion.identity);
        sun.setTargetY(transform.position.y - 0.3f);
        sun.setState(SunState.ProducedbySunflower);
    }
}
