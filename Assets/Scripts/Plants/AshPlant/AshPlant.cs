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

    public string attackPlaceName = "AttackPlace";
    protected Collider2D attackPlaceCollider;
    protected List<Zombie> targets;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 1800;
        targets = new List<Zombie>();
        // 设置子物体碰撞器
        attackPlaceCollider = transform.Find(attackPlaceName).GetComponent<Collider2D>();
        attackPlaceCollider.GetComponent<TriggerForwarder>().SetPlantParentHandler(this);
        attackPlaceCollider.enabled = false;
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        switch (state)
        {
            case PlantState.Suspension:
                attackPlaceCollider.enabled = false;
                break;
            case PlantState.Idle:
                attackPlaceCollider.enabled = true;
                break;
            case PlantState.Die:
                attackPlaceCollider.enabled = false;
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

    protected virtual void setExplode()
    {
        anim.SetTrigger(AnimatorConfig.plant_Explode);
    }

    protected virtual void Explode()
    {
        if (explodeEffect) GameObject.Instantiate(explodeEffect, transform.position, Quaternion.identity);
        foreach (Zombie zombie in targets) zombie.UnderAttack(attackPoint, attackDieMode);
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
