using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewspaperZombieState
{
    None, ReadPaper, Gasp, NoPaper
}

public class NewspaperZombie : Zombie
{
    protected NewspaperZombieState newspaperZombieState;

    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.NewspaperZombie;
        spawnWeight = 2.0f;
        maxArmor2Health = 150; currArmor2Health = maxArmor2Health;

        attackPoint = 60;
        setNewspaperZombieState(NewspaperZombieState.ReadPaper);
    }

    private void setNewspaperZombieState(NewspaperZombieState state)
    {
        if (newspaperZombieState == state) return;
        newspaperZombieState = state;
        switch (state)
        {
            case NewspaperZombieState.ReadPaper:
                baseSpeed = 0.2f;
                speed = Random.Range(1.0f, 2.0f) * baseSpeed;
                speedLevel = (speed - baseSpeed) / baseSpeed;
                break;
            case NewspaperZombieState.Gasp:
                transform.DOKill(); currentMoveTween = null;
                AudioManager.Instance.playClip(ResourceConfig.sound_zombie_newspaperRip);
                break;
            case NewspaperZombieState.NoPaper:
                anim.SetTrigger(AnimatorConfig.zombie_stopEffect);
                AudioManager.Instance.playClip(ResourceConfig.sound_zombie_newspaperRarrghs[Random.Range(0, ResourceConfig.sound_zombie_newspaperRarrghs.Length)]);
                baseSpeed = 0.1f;
                speed = Random.Range(0.5f, 0.6f);
                speedLevel = (speed - 0.5f) / baseSpeed;
                moveToHouse();
                break;
            default:
                break;
        }
    }

    public override void AddArmor2Health(int point)
    {
        base.AddArmor2Health(point);
        if (currArmor2Health <= 0) setNewspaperZombieState(NewspaperZombieState.Gasp);
    }
}
