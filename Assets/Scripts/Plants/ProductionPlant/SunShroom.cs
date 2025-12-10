using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SunShroomState
{
    None, Small, Growing, Big
}

public class SunShroom : ProductionPlant
{
    protected SunID sunID;
    protected Sun sunPrefab;
    protected SunShroomState sunShroomState;
    protected float growTime, growTimer;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.SunShroom;
        type = PlantType.Normal;
        produceTimer = 17.0f + Random.Range(0.0f, 2.0f);
        sunID = SunID.Small;
        sunPrefab = PrefabSystem.Instance.GetSunPrefab(sunID);
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
        growTime = 120.0f; growTimer = 0.0f;
        setSunShroomState(SunShroomState.Small);
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        if (canAct() && sunShroomState == SunShroomState.Small)
        {
            growTimer += Time.deltaTime;
            if (growTimer > growTime)
            {
                setSunShroomState(SunShroomState.Growing);
                growTimer = 0.0f;
            }
        }
    }

    private void setSunShroomState(SunShroomState state)
    {
        if (sunShroomState == state) return;
        sunShroomState = state;
        switch (state)
        {
            case SunShroomState.Small:
                sunID = SunID.Small;
                sunPrefab = PrefabSystem.Instance.GetSunPrefab(sunID);
                break;
            case SunShroomState.Growing:
                anim.SetTrigger(AnimatorConfig.plant_grow);
                AudioManager.Instance.playClip(ResourceConfig.sound_plant_plantgrow);
                break;
            case SunShroomState.Big:
                sunID = SunID.Medium;
                sunPrefab = PrefabSystem.Instance.GetSunPrefab(sunID);
                break;
            default:
                break;
        }
    }

    protected override void Produce()
    {
        Sun sun = GameObject.Instantiate(sunPrefab, producePlace.position, Quaternion.identity);
        sun.setTargetY(transform.position.y);
        sun.setState(SunState.ProducedbySunflower);
    }
}
