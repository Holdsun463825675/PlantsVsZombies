using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ChomperState
{
    None, Ready, CoolingDown, Recovery
}

public class Chomper : AttackPlant
{
    protected int attackPoint;
    protected float coolingDownTime, coolingDownTimer;
    protected int attackDieMode = 2;
    protected ChomperState chomperState;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Chomper;
        type = PlantType.Normal;
        attackPoint = 40;
        coolingDownTime = 30.0f; coolingDownTimer = 0.0f;
        attackTime = 0.0f;
        chomperState = ChomperState.Ready;
    }

    protected override void IdleUpdate()
    {
        switch (chomperState)
        {
            case ChomperState.Ready:
                if (canAct() && HaveAttackTarget()) setAttack();
                break;
            case ChomperState.CoolingDown:
                coolingDownTimer += Time.deltaTime;
                if (coolingDownTimer >= coolingDownTime)
                {
                    setChomperState(ChomperState.Recovery);
                    coolingDownTimer = 0.0f;
                }
                break;
            default:
                break;
        }
    }

    private void setChomperState(ChomperState state)
    {
        if (chomperState == state) return;
        chomperState = state;
        switch (state)
        {
            case ChomperState.Ready:
                anim.SetTrigger(AnimatorConfig.plant_ready);
                break;
            case ChomperState.CoolingDown:
                anim.SetTrigger(AnimatorConfig.plant_coolingDown);
                break;
            case ChomperState.Recovery:
                anim.SetTrigger(AnimatorConfig.plant_recovery);
                break;
            default:
                break;
        }
    }

    protected override bool HaveAttackTarget()
    {
        foreach (Zombie target in targetZombie)
        {
            if (!(target.row == 0 || targetRows.Contains(0) || targetRows.Contains(target.row))) continue; // 只攻击可攻击行的僵尸
            if (target.isHealthy() && (target.isPlantKill || target.isBulletHit)) return true; // 不掉头时才攻击
        }
        return false;
    }

    private bool CanKill(Zombie zombie)
    {
        return (zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row)) && zombie.isHealthy() && zombie.isPlantKill;
    }

    protected override bool CanAttack(Zombie zombie)
    {
        return (zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row)) && zombie.isHealthy() && !zombie.isPlantKill && zombie.isBulletHit;
    }


    protected override void Attack()
    {
        base.Attack();
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bigchomp);
        // 不掉头且能被机制杀的僵尸作为目标
        Zombie target_isPlantKill = targetZombie.FirstOrDefault(zombie => CanKill(zombie));
        if (target_isPlantKill != null) // 能吃则机制杀
        {
            target_isPlantKill.kill(attackDieMode);
            setChomperState(ChomperState.CoolingDown);
        }
        else // 不能吃则打伤害，算子弹伤害
        {
            // 不掉头的僵尸作为目标
            Zombie target = targetZombie.FirstOrDefault(zombie => CanAttack(zombie));
            if (target != null) target.UnderAttack(attackPoint); 
            anim.SetTrigger(AnimatorConfig.plant_ready); // 返回Idle动画
        }
        anim.ResetTrigger(AnimatorConfig.plant_attack); // 取消攻击触发器
    }
}
