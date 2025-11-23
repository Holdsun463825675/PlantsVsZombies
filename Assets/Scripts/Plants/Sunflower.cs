using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{
    private float produceTime;
    private float produceTimer;

    public Sun sunPrefab;

    protected override void Awake()
    {
        base.Awake();
        produceTime = 23.0f + Random.Range(0.0f, 2.0f);
        produceTimer = 18.0f;
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        produceTimer += Time.deltaTime;
        if (produceTimer >= produceTime)
        {
            setState(PlantState.Effect);
            produceTimer = 0.0f;
            produceTime = 23.0f + Random.Range(0.0f, 2.0f);
        }
    }

    public void produceSun()
    {
        Sun sun = GameObject.Instantiate(sunPrefab, transform.position, Quaternion.identity);
        sun.setTargetY(transform.position.y - 0.3f);
        sun.setState(SunState.ProducedbySunflower);
    }
}
