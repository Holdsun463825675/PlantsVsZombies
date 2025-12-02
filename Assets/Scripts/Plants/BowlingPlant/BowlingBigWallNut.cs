using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBigWallNut : BowlingPlant
{
    protected override void Awake()
    {
        base.Awake();
        id = PlantID.BowlingBigWallNut;
        type = PlantType.Normal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie) zombie.UnderAttack(attackPoint);
                break;
            case TagConfig.armor2:
                Armor2 armor2 = collision.GetComponent<Armor2>();
                if (armor2) armor2.UnderAttack(attackPoint);
                break;
            default:
                break;
        }
    }
}
