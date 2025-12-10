using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPea : AttackPlant
{
    private int attackPeaNum = 4;
    private float shootInterval = 0.15f;
    private List<GameObject> assList; // ΩË÷˙DOTween µœ÷…‰ª˜º‰∏Ù

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.GatlingPea;
        type = PlantType.Normal;
        prePlantID = PlantID.Repeater;
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        assList = new List<GameObject>();
        bulletID = BulletID.Pea;
        bulletPrefab = PrefabSystem.Instance.GetBulletPrefab(bulletID);
    }

    public override void Pause()
    {
        base.Pause();
        foreach (GameObject ass in assList) ass.transform.DOPause();
    }

    public override void Continue()
    {
        base.Continue();
        foreach (GameObject ass in assList) ass.transform.DOPlay();
    }

    private void shootPea()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plantshoot_throw);
        Bullet pea = GameObject.Instantiate(bulletPrefab, effectPlace.position, Quaternion.identity);
        pea.setBulletType(BulletType.Shoot); pea.setBulletDirection(BulletDirection.Right); pea.setTargetRows(targetRows);
        pea.setState(BulletState.ToBeUsed);
        float target_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
        pea.moveToPlace(new Vector3(target_x, effectPlace.position.y, effectPlace.position.z));
    }

    private void shoot(int num)
    {
        shootPea();
        if (num > 1)
        {
            GameObject ass = new GameObject($"{id}_ShootTimer");
            assList.Add(ass);
            ass.transform.DOMove(transform.position, shootInterval)
                .OnComplete(() => {
                    shoot(num - 1);
                    assList.Remove(ass);
                    Destroy(ass);
                });
            if (GameManager.Instance.state == GameState.Paused) ass.transform.DOPause();
        }
    }

    protected override void Attack()
    {
        shoot(attackPeaNum);
    }
}
