using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffShroom : AttackPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.PuffShroom;
        type = PlantType.Normal;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        bulletID = BulletID.Spore;
        bulletPrefab = PrefabSystem.Instance.GetBulletPrefab(bulletID);
    }

    protected override void Attack()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_puff);
        Bullet bullet = GameObject.Instantiate(bulletPrefab, effectPlace.position, Quaternion.identity);
        bullet.setBulletType(BulletType.Shoot); bullet.setBulletDirection(BulletDirection.Right); bullet.setTargetRows(targetRows);
        bullet.setState(BulletState.ToBeUsed);
        float target_x = bulletTarget.position.x;
        bullet.moveToPlace(new Vector3(target_x, effectPlace.position.y, effectPlace.position.z));
    }
}
