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
        attackPoint = 500;
        currMoveDirection = 0;
        preTargetRow = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie: // 先判定僵尸
                targetZombie = collision.GetComponent<Zombie>();
                AttackZombie();
                break;
            case TagConfig.armor2: 
                targetArmor2 = collision.GetComponent<Armor2>();
                AttackArmor2();
                break;
            default:
                break;
        }
    }

    protected virtual void AttackZombie()
    {
        if (!targetZombie || preTargetRow == targetZombie.row) return;
        targetZombie.UnderAttack(attackPoint);
        transMove(targetZombie.row); // 攻击后改变运动方向
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetZombie.underAttackSound, targetZombie.underAttackSoundPriority);
    }

    protected virtual void AttackArmor2()
    {
        Debug.Log(targetArmor2.zombie.row);
        if (!targetArmor2 || preTargetRow == targetArmor2.zombie.row) return;
        if (targetZombie && targetZombie.armor2 == targetArmor2) // 优先攻击僵尸
        {
            AttackZombie(); return;
        }
        targetArmor2.UnderAttack(attackPoint);
        transMove(targetArmor2.zombie.row); // 攻击后改变运动方向
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetArmor2.underAttackSound, targetArmor2.underAttackSoundPriority);
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
