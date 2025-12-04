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
                if (targets.Count > 0) setAttack();
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

    protected override void Attack()
    {
        base.Attack();
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_bigchomp);
        Zombie target_isPlantKill = targets.FirstOrDefault(zombie => zombie.isPlantKill);
        if (target_isPlantKill != null) // 能吃则机制杀
        {
            target_isPlantKill.kill(attackDieMode);
            setChomperState(ChomperState.CoolingDown);
        }
        else // 不能吃则打伤害
        {
            Zombie target = targets.FirstOrDefault(zombie => !zombie.isPlantKill);
            if (target != null) target.UnderAttack(attackPoint); 
            anim.SetTrigger(AnimatorConfig.plant_ready); // 返回Idle动画
        }
        anim.ResetTrigger(AnimatorConfig.plant_attack); // 取消攻击触发器
    }
}
