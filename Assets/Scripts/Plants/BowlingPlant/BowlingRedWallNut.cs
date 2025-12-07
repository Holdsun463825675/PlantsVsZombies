using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingRedWallNut : BowlingPlant
{
    public Effect explodeEffect;
    protected int attackDieMode = 1;

    protected List<Zombie> targetZombie;
    protected List<Armor2> targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.BowlingRedWallNut;
        type = PlantType.Normal;
        targetZombie = new List<Zombie>();
        targetArmor2 = new List<Armor2>();
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
                targetRows = new List<int> { Mathf.Max(1, row - 1), row, Mathf.Min(row + 1, CellManager.Instance.maxRow) }; // 只能攻击本行、上一行、下一行
                break;
            default:
                break;
        }
    }

    protected virtual void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bowlingimpact2);
        if (explodeEffect) GameObject.Instantiate(explodeEffect, transform.position, Quaternion.identity);
        foreach (Armor2 armor2 in targetArmor2) if (CanAttack(armor2)) armor2.UnderAttack(attackPoint, attackDieMode);
        foreach (Zombie zombie in targetZombie) if (CanAttack(zombie)) zombie.UnderAttack(attackPoint, attackDieMode);
        setState(PlantState.Die);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if ((zombie.row == 0 || zombie.row == row) && zombie.isHealthy() && zombie.isBulletHit) Explode();
                break;
            case TagConfig.armor2:
                Armor2 armor2 = collision.GetComponent<Armor2>();
                if (armor2.zombie.row == 0 || armor2.zombie.row == row) Explode();
                break;
            default:
                break;
        }
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
