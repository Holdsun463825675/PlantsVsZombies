using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPlant : Plant
{
    protected float attackTime;
    protected float attackTimer;

    protected List<Zombie> targets;

    protected override void Awake()
    {
        base.Awake();
        attackTime = 1.5f; attackTimer = Random.Range(0, attackTime);
        targets = new List<Zombie>();
    }


    protected override void IdleUpdate()
    {
        base.IdleUpdate();

        attackTimer += Time.deltaTime;
        // TODO: ะþัง
        if (attackTimer >= attackTime / Time.timeScale)
        {
            if (effectPlace && targets.Count != 0) setAttack();
            attackTimer = 0.0f;
        }
    }

    protected virtual void setAttack()
    {
        anim.SetTrigger(AnimatorConfig.plant_Attack);
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
