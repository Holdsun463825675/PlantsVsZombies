using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlingPlant : Plant
{
    protected int attackPoint;
    protected List<int> targetRows; // 可攻击的行，0为任意，大于0为行数
    protected float speed;
    protected Vector3 target_position;

    public BulletHitSound hitSound;
    public int hitSoundPriority;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 1800;
        targetRows = new List<int> { 0 }; // 默认为任意行都可攻击
        speed = 3.0f;
        hitSound = BulletHitSound.Bowling;
        hitSoundPriority = 2;
    }

    public override void Pause()
    {
        base.Pause();
        transform.DOPause();
    }

    public override void Continue()
    {
        base.Continue();
        transform.DOPlay();
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        switch (state)
        {
            case PlantState.Suspension:
                transform.DOKill();
                break;
            case PlantState.Idle:
                spriteRenderer.sortingLayerName = "Bullet";
                if (HPText)
                {
                    HPText.sortingLayerID = spriteRenderer.sortingLayerID;
                    HPText.sortingOrder = spriteRenderer.sortingOrder + 1;
                } 
                float target_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
                target_position = new Vector3(target_x, transform.position.y, transform.position.z);
                moveToPlace(target_position);
                break;
            case PlantState.Die:
                spriteRenderer.sortingLayerName = "Bullet";
                if (HPText)
                {
                    HPText.sortingLayerID = spriteRenderer.sortingLayerID;
                    HPText.sortingOrder = spriteRenderer.sortingOrder + 1;
                }
                transform.DOKill();
                break;
            default:
                break;
        }
    }

    public void moveToPlace(Vector3 position, float speed = 3.0f)
    {
        if (cell) cell.removePlant(this); // 移除种植
        this.speed = speed;
        target_position = position;
        transform.DOMove(position, this.speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => setState(PlantState.Die));
        if (GameManager.Instance.state == GameState.Paused) transform.DOPause();
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bowling);
    }

    protected virtual bool HaveAttackTarget()
    {
        return true;
    }

    protected virtual bool CanAttack(Zombie zombie)
    {
        return zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row);
    }

    protected virtual bool CanAttack(Armor2 armor2)
    {
        return armor2.zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(armor2.zombie.row);
    }

}
