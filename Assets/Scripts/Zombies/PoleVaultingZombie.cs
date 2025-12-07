using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PoleVaultingZombieState
{
    None, NotEffect, Effect, StopEffect
}


public class PoleVaultingZombie : Zombie
{
    private PoleVaultingZombieState poleVaultingZombieState;
    private Transform realPosition; // 跳跃时真实位置
    private float jumpTime = 2.2f;
    private float jumpPositionXOffset = 2.6f; // 跳跃时x位置的偏移
    private Vector3 target;

    protected override void Awake()
    {
        base.Awake();
        zombieID = ZombieID.PoleVaultingZombie;
        spawnWeight = 2.0f;
        maxHealth = 500; currHealth = maxHealth;
        attackPoint = 60;
        setPoleVaultingZombieState(PoleVaultingZombieState.NotEffect);
    }

    public override void Pause()
    {
        base.Pause();
        if (realPosition) realPosition.DOPause();
    }

    public override void Continue()
    {
        base.Continue();
        if (realPosition) realPosition.DOPlay();
    }

    private void setPoleVaultingZombieState(PoleVaultingZombieState state)
    {
        if (poleVaultingZombieState == state) return;
        poleVaultingZombieState = state;
        switch (state)
        {
            case PoleVaultingZombieState.NotEffect:
                isPlantKill = true; isBulletHit = true;
                baseSpeed = 0.1f;
                speed = Random.Range(0.5f, 0.6f);
                speedLevel = (speed - 0.5f) / baseSpeed;
                break;
            case PoleVaultingZombieState.Effect:
                transform.DOKill(); currentMoveTween = null;
                isPlantKill = false; isBulletHit = false; // 无法被机制杀、子弹命中
                shadow.SetActive(false);

                Vector3 jumpOverplace = transform.position;
                foreach (Plant plant in effectTargets) if (plant && CanEffect(plant)) jumpOverplace = plant.jumpOverPlace.position;
                foreach (Plant plant in effectBowlingTargets) if (plant && CanEffect(plant)) jumpOverplace = plant.jumpOverPlace.position;

                target = new Vector3(jumpOverplace.x, transform.position.y, transform.position.z);
                // 创建真实位置
                GameObject newObject = new GameObject($"{zombieID}_realPosition");
                realPosition = newObject.transform;
                realPosition.position = transform.position;
                realPosition.DOMove(target, jumpTime).SetEase(Ease.Linear);
                //transform.DOMove(target, jumpTime).SetEase(Ease.Linear); // 仅僵尸位置真实移动
                //transform.DOMove(new Vector3(target.x + jumpPositionXOffset, transform.position.y, transform.position.z), jumpTime)
                //    .SetEase(Ease.Linear); // 直接僵尸位置贴合动画移动
                transform.DOMove(target, 20.0f).SetEase(Ease.Linear); // 僵尸位置真实移动一点，便于后续判断碰撞

                anim.SetTrigger(AnimatorConfig.zombie_effect);
                break;
            case PoleVaultingZombieState.StopEffect:
                anim.SetTrigger(AnimatorConfig.zombie_stopEffect);
                transform.DOKill(); currentMoveTween = null;
                if (realPosition)
                {
                    realPosition.DOKill();
                    transform.position = realPosition.position;
                } 
                isPlantKill = true; isBulletHit = true;
                shadow.SetActive(true);

                baseSpeed = 0.2f;
                speed = Random.Range(1.0f, 2.0f) * baseSpeed;
                speedLevel = (speed - baseSpeed) / baseSpeed;
                moveToHouse();
                break;
            default:
                break;
        }
    }

    protected override void kinematicsUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false) return;
        switch (poleVaultingZombieState)
        {
            case PoleVaultingZombieState.NotEffect:
                if (HaveEffectTarget()) setPoleVaultingZombieState(PoleVaultingZombieState.Effect);
                break;
            case PoleVaultingZombieState.Effect:
                break;
            case PoleVaultingZombieState.StopEffect:
                Plant target = getAttackTarget();
                if (target)
                {
                    setMoveState(ZombieMoveState.Stop);
                    anim.SetBool(AnimatorConfig.zombie_isAttack, true);
                    return;
                }
                anim.SetBool(AnimatorConfig.zombie_isAttack, false);
                break;
            default:
                break;
        }
    }

    private void playEffectSound()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_zombie_poleVault);
    }

    private void effectJudge()
    {
        bool judge = true;
        foreach (Plant plant in effectTargets) if (plant && CanAttack(plant) && !plant.isZombieJumpOver) judge = false;
        foreach (Plant plant in effectBowlingTargets) if (plant && CanAttack(plant) && !plant.isZombieJumpOver) judge = false;
        if (!judge)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_zombie_bonk);
            setPoleVaultingZombieState(PoleVaultingZombieState.StopEffect);
        }
        else
        {
            transform.DOKill();
            transform.DOMove(new Vector3(target.x + jumpPositionXOffset, transform.position.y, transform.position.z), jumpTime / 2)
                .SetEase(Ease.Linear); // 僵尸位置贴合动画移动
        }
    }

    protected override void DieUpdate()
    {
        base.DieUpdate();
        if (poleVaultingZombieState == PoleVaultingZombieState.Effect && realPosition)
        {
            realPosition.DOKill();
            transform.position = realPosition.position;
        }
    }
}
