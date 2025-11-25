using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProductionPlant : Plant
{
    protected float produceTime;
    protected float produceTimer;

    protected override void Awake()
    {
        base.Awake();
        produceTime = 24.0f; produceTimer = 0.0f;
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
        anim.SetTrigger(AnimatorConfig.plant_Produce);
    }

    protected virtual void Produce()
    {

    }
}
