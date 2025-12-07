using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class AttackPlant : Plant
{
    protected float attackTime;
    protected float attackTimer;
    protected List<int> targetRows; // 可攻击的行，0为任意，大于0为行数

    protected List<Zombie> targets;

    protected override void Awake()
    {
        base.Awake();
        attackTime = 1.5f; attackTimer = Random.Range(0, attackTime);
        targetRows = new List<int>();
        targets = new List<Zombie>();
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) targetRows = new List<int> { row }; // 默认只能攻击本行
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        attackTimer += Time.deltaTime;
        // TODO: 时间玄学
        if (attackTimer >= attackTime / Time.timeScale)
        {
            if (effectPlace && HaveAttackTarget()) setAttack();
            attackTimer = 0.0f;
        }
    }

    protected virtual bool HaveAttackTarget() // 在可攻击的行是否有目标
    {
        foreach (Zombie target in targets) // 只攻击可攻击行的僵尸
        {
            if (target.row == 0 || targetRows.Contains(0) || targetRows.Contains(target.row)) return true;
        }
        return false;
    }

    protected virtual void setAttack()
    {
        anim.SetTrigger(AnimatorConfig.plant_attack);
    }

    protected virtual void Attack()
    {

    }

    public override void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            Zombie target = collision.GetComponent<Zombie>();
            if (target && !targets.Contains(target)) targets.Add(target);
        }
    }

    public override void OnChildTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            Zombie target = collision.GetComponent<Zombie>();
            if (target) targets.Remove(target);
        }
    }
}
