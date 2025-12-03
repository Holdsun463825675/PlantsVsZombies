using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BowlingWallNut : BowlingPlant
{
    private int currMoveDirection; // 当前移动方向，0向右，1向上，-1向下
    private int preTargetRow; // 上一个攻击目标所处行

    protected Zombie targetZombie;
    protected Armor2 targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.BowlingWallNut;
        type = PlantType.Normal;
        attackPoint = 500;
        currMoveDirection = 0;
        preTargetRow = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.bowling_zombie:
                targetZombie = collision.GetComponent<Transform>().parent.GetComponent<Zombie>();
                if (targetZombie != null)
                {
                    AttackZombie();
                    return;
                } 
                targetArmor2 = collision.GetComponent<Transform>().parent.GetComponent<Armor2>();
                if (targetArmor2 != null)
                {
                    AttackArmor2();
                    return;
                } 
                break;
            default:
                break;
        }
    }

    protected virtual void AttackZombie()
    {
        if (targetZombie == null || preTargetRow == targetZombie.row) return;
        targetZombie.UnderAttack(attackPoint);
        transMove(targetZombie.row); // 攻击后改变运动方向
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetZombie.underAttackSound, targetZombie.underAttackSoundPriority);
        targetZombie = null; targetArmor2 = null; // 清空目标
    }

    protected virtual void AttackArmor2()
    {
        if (targetArmor2 == null || preTargetRow == targetArmor2.zombie.row) return;
        if (targetZombie && targetZombie.armor2 == targetArmor2) // 优先攻击僵尸
        {
            AttackZombie(); return;
        }
        targetArmor2.UnderAttack(attackPoint);
        transMove(targetArmor2.zombie.row); // 攻击后改变运动方向
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetArmor2.underAttackSound, targetArmor2.underAttackSoundPriority);
        targetZombie = null; targetArmor2 = null; // 清空目标
    }

    private void transMove(int preTargetRow=0)
    {
        transform.DOKill(); // 结束当前运动

        if (currMoveDirection == 0) // 根据所在行改变运动方向
        {
            if (row == 1) currMoveDirection = -1;
            else if (row == MapManager.Instance.currMap.maxRow) currMoveDirection = 1;
            else currMoveDirection = Random.Range(0, 2) * 2 - 1; // 随机方向
        } 
        else currMoveDirection = -currMoveDirection;

        this.preTargetRow = preTargetRow; // 重置上一个攻击目标所在行

        // 计算新目标位置
        Vector3 newDirection = new Vector3(1, currMoveDirection, 0);
        float target_y = transform.position.y;
        if (currMoveDirection == 1) target_y = MapManager.Instance.currMap.endlinePositions[2].position.y;
        else target_y = MapManager.Instance.currMap.endlinePositions[3].position.y;
        target_position = transform.position + newDirection * Mathf.Abs(target_y - transform.position.y);

        transform.DOMove(target_position, speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                // 销毁或改变运动方向
                if (target_position.x >= MapManager.Instance.currMap.endlinePositions[1].position.x) setState(PlantState.Die);
                else transMove();
            });
        if (GameManager.Instance.state == GameState.Paused) transform.DOPause();
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bowling);
    }
}
