using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : AttackPlant
{
    public Pea peaPrefab;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.PeaShooter;
        type = PlantType.Normal;
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
    }

    protected override void Attack()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plantshoot_throw);
        Pea pea = GameObject.Instantiate(peaPrefab, effectPlace.position, Quaternion.identity);
        pea.setTargetRows(targetRows);
        pea.setState(BulletState.ToBeUsed);
        float target_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
        pea.moveToPlace(new Vector3(target_x, effectPlace.position.y, effectPlace.position.z));
    }

}
