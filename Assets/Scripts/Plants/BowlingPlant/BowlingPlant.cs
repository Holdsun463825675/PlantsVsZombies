using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlingPlant : Plant
{
    protected int attackPoint;
    protected float speed;
    protected Vector3 target_position;

    public BulletHitSound hitSound;
    public int hitSoundPriority;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 1800;
        speed = 3.0f;
        type = PlantType.None; // 无种植类型
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
        this.speed = speed;
        target_position = position;
        transform.DOMove(position, this.speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => setState(PlantState.Die));
        if (GameManager.Instance.state == GameState.Paused) transform.DOPause();
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bowling);
    }

}
