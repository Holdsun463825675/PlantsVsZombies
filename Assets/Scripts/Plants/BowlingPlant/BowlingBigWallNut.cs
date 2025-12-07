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

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) targetRows = new List<int> { row }; // 只能攻击本行
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie && CanAttack(zombie))
                {
                    AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, zombie.underAttackSound, zombie.underAttackSoundPriority);
                    zombie.UnderAttack(attackPoint);
                } 
                break;
            case TagConfig.armor2:
                Armor2 armor2 = collision.GetComponent<Armor2>();
                if (armor2 && CanAttack(armor2))
                {
                    AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, armor2.underAttackSound, armor2.underAttackSoundPriority);
                    armor2.UnderAttack(attackPoint);
                } 
                break;
            default:
                break;
        }
    }
}
