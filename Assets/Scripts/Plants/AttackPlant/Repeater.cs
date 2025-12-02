using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : AttackPlant
{
    public Pea peaPrefab;
    private int attackPeaNum = 2;
    private float shootInterval = 0.2f;
    private List<GameObject> assList; // ΩË÷˙DOTween µœ÷…‰ª˜º‰∏Ù

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Repeater;
        type = PlantType.Normal;
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        assList = new List<GameObject>();
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
        Pea pea = GameObject.Instantiate(peaPrefab, effectPlace.position, Quaternion.identity);
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
        }
    }

    protected override void Attack()
    {
        shoot(attackPeaNum);
    }
}
