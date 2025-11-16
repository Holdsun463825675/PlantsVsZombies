using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plant
{
    private float shootTime;
    private float shootTimer;

    public Pea peaPrefab;

    private Transform shootPlace;


    protected override void Awake()
    {
        base.Awake();
        shootTime = 1.5f;
        shootTimer = Random.Range(0.0f, shootTime);
        shootPlace = transform.Find("ShootPlace");
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        shootTimer += Time.fixedDeltaTime;
        if (shootTimer >= shootTime)
        {
            setState(PlantState.Effect);
            shootTimer = 0.0f;
        }
    }

    public void shoot()
    {
        if (!shootPlace || targets.Count == 0) return;
        AudioManager.Instance.playClip(ResourceConfig.sound_plantshoot_throw);
        Pea pea = GameObject.Instantiate(peaPrefab, shootPlace.position, Quaternion.identity);
        pea.setState(PeaState.ToBeUsed);
    }

}
