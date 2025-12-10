using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : AttackPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.PeaShooter;
        type = PlantType.Normal;
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        bulletID = BulletID.Pea;
        bulletPrefab = PrefabSystem.Instance.GetBulletPrefab(bulletID);
    }

    protected override void Attack()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plantshoot_throw);
        Bullet pea = GameObject.Instantiate(bulletPrefab, effectPlace.position, Quaternion.identity);
        pea.setBulletType(BulletType.Shoot); pea.setBulletDirection(BulletDirection.Right); pea.setTargetRows(targetRows);
        pea.setState(BulletState.ToBeUsed);
        float target_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
        pea.moveToPlace(new Vector3(target_x, effectPlace.position.y, effectPlace.position.z));
    }

}
