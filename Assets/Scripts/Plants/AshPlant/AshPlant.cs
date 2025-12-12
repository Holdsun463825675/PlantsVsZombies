using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum AshPlantState
{
    Normal,
    Explode,
}

public class AshPlant : Plant
{
    public Effect explodeEffect;

    protected int attackPoint;
    protected int attackMode = 1;
    protected List<int> targetRows; // 可攻击的行，0为任意，大于0为行数
    protected AshPlantState ashState;

    protected List<Zombie> targetZombie;
    protected List<Armor2> targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 1800;
        targetRows = new List<int> { 0 }; // 默认为任意行都可攻击
        targetZombie = new List<Zombie>();
        targetArmor2 = new List<Armor2>();
        ashState = AshPlantState.Normal;
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        switch (state)
        {
            case PlantState.Suspension:
                // 提前激活碰撞体
                if (state == PlantState.Suspension) effectPlaceCollider.enabled = true;
                break;
            case PlantState.Idle:
                break;
            default:
                break;
        }
    }

    public virtual void setAshState(AshPlantState state)
    {
        if (ashState == state) return;
        ashState = state;
        switch (state)
        {
            case (AshPlantState.Explode):
                setExplode();
                break;
            default:
                break;
        }
    }

    public override void Pause()
    {
        base.Pause();
    }

    public override void Continue()
    {
        base.Continue();
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

    protected virtual void setExplode()
    {
        anim.SetTrigger(AnimatorConfig.plant_explode);
        // 移动一点点激活碰撞体判定
        //transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1e-3f, transform.position.z), 1f);
    }

    protected virtual void ExplodeEffect()
    {
        if (!explodeEffect) return;
        GameObject.Instantiate(explodeEffect, transform.position, Quaternion.identity);
    }

    protected virtual void Explode()
    {
        ExplodeEffect();
        List<Armor2> targetArmor2s = new List<Armor2>(targetArmor2);
        List<Zombie> targetZombies = new List<Zombie>(targetZombie);
        foreach (Armor2 armor2 in targetArmor2s) if (CanAttack(armor2)) armor2.UnderAttack(attackPoint, attackMode);
        foreach (Zombie zombie in targetZombies) if (CanAttack(zombie)) zombie.UnderAttack(attackPoint, attackMode);
        kill();
    }

    protected override void AddHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0)
        {
            if (ashState == AshPlantState.Explode) Explode(); // 爆炸时死亡直接爆炸
            else setState(PlantState.Die);
        } 
    }

    public override void UnderAttack(int point, int mode=0, Zombie zombie=null)
    {
        if (ashState == AshPlantState.Explode && mode == 0) // 爆炸时不受啃咬伤害，只有音效
        {
            playUnderAttackSound();
            return;
        }
        base.UnderAttack(point, mode);
    }

    public override void OnChildTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie && !targetZombie.Contains(zombie)) targetZombie.Add(zombie);
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
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie) targetZombie.Remove(zombie);
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
