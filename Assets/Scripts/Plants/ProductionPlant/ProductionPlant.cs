using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProductionPlant : Plant
{
    protected float produceTime;
    protected float produceTimer;

    public string producePlaceName = "ProducePlace";
    protected Transform producePlace;

    protected override void Awake()
    {
        base.Awake();
        produceTime = 24.0f; produceTimer = 0.0f;
        producePlace = transform.Find(producePlaceName).GetComponent<Transform>();
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        produceTimer += Time.deltaTime;

        if (produceTimer >= produceTime)
        {
            setProduce();
            produceTimer = 0.0f;
        }
    }

    protected virtual void setProduce()
    {
        anim.SetTrigger(AnimatorConfig.plant_produce);
    }

    protected virtual void Produce()
    {

    }
}
