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
        Vector3 sunPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Sun sun = GameObject.Instantiate(sunPrefab, sunPosition, Quaternion.identity);
        sun.setTargetY(transform.position.y);
        sun.setState(SunState.ProducedbySunflower);
    }
}
