using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class AttackPlant : Plant
{
    public Effect attackEffect; // 攻击特效
    protected Effect currAttackEffect;
    public Transform bulletTarget;

    protected float attackTime;
    protected float attackTimer;
    protected BulletID bulletID;
    protected Bullet bulletPrefab;
    protected List<int> targetRows; // 可攻击的行，0为任意，大于0为行数

    protected List<Zombie> targetZombie;
    protected List<Armor2> targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        attackTime = 1.5f; attackTimer = Random.Range(0, attackTime);
        bulletID = BulletID.None;
        targetRows = new List<int>();
        targetZombie = new List<Zombie>();
        targetArmor2 = new List<Armor2>();
        bulletTarget = transform.Find("BulletTarget");
    }

    public override void Pause()
    {
        base.Pause();
        if (currAttackEffect) currAttackEffect.GetComponent<Animator>().enabled = false;
    }

    public override void Continue()
    {
        base.Continue();
        if (currAttackEffect) currAttackEffect.GetComponent<Animator>().enabled = true;
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
            if (canAct() && effectPlace && HaveAttackTarget()) setAttack();
            attackTimer = 0.0f;
        }
    }

    protected virtual bool HaveAttackTarget() // 在可攻击的行是否有目标
    {
        foreach (Zombie target in targetZombie) // 只攻击可攻击行的僵尸
        {
            if ((target.row == 0 || targetRows.Contains(0) || targetRows.Contains(target.row)) && target.isBulletHit) return true;
        }
        return false;
    }

    protected virtual bool CanAttack(Zombie zombie)
    {
        return (zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row)) && zombie.isBulletHit;
    }

    protected virtual bool CanAttack(Armor2 armor2)
    {
        return armor2.zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(armor2.zombie.row);
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
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie target = collision.GetComponent<Zombie>();
                if (target && !targetZombie.Contains(target)) targetZombie.Add(target);
                break;
            case TagConfig.armor2:
                Armor2 armor2 = collision.GetComponent<Armor2>();
                if (armor2 && !targetArmor2.Contains(armor2)) targetArmor2.Add(armor2);
                break;
            default:
                break;
        }
    }

    public override void OnChildTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie target = collision.GetComponent<Zombie>();
                if (target) targetZombie.Remove(target);
                break;
            case TagConfig.armor2:
                Armor2 armor2 = collision.GetComponent<Armor2>();
                if (armor2) targetArmor2.Remove(armor2);
                break;
            default:
                break;
        }
    }
}
