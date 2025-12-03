using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AshPlantState
{
    Normal,
    Explode,
}

public class AshPlant : Plant
{
    public Effect explodeEffect;

    protected int attackPoint;
    protected int attackDieMode = 1;
    protected AshPlantState ashState;

    protected List<Zombie> targetZombie;
    protected List<Armor2> targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 1800;
        targetZombie = new List<Zombie>();
        targetArmor2 = new List<Armor2>();
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

    protected virtual void setExplode()
    {
        anim.SetTrigger(AnimatorConfig.plant_Explode);
        // 移动一点点激活碰撞体判定
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1e-3f, transform.position.z), 1f);
    }

    protected virtual void Explode()
    {
        if (explodeEffect) GameObject.Instantiate(explodeEffect, transform.position, Quaternion.identity);
        List<Armor2> targetArmor2s = new List<Armor2>(targetArmor2);
        List<Zombie> targetZombies = new List<Zombie>(targetZombie);
        foreach (Armor2 armor2 in targetArmor2s) armor2.UnderAttack(attackPoint, attackDieMode);
        foreach (Zombie zombie in targetZombies) zombie.UnderAttack(attackPoint, attackDieMode);
        setState(PlantState.Die);
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

    public override void UnderAttack(int point, int type=0)
    {
        if (ashState == AshPlantState.Explode && type == 0) // 爆炸时不受啃咬伤害，只有音效
        {
            playUnderAttackSound();
            return;
        }
        base.UnderAttack(point, type);
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
