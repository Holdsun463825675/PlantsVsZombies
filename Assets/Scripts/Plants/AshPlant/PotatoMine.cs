using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PotatoMineState
{
    None, CoolingDown, Recovery, Ready
}

public class PotatoMine : AshPlant
{
    protected float coolingDownTime, coolingDownTimer;
    protected int attackMode = 2;
    protected PotatoMineState potatoMineState;
    protected List<Zombie> explodeTargets;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.PotatoMine;
        type = PlantType.Normal;
        attackDieMode = 2;
        coolingDownTime = 15.0f; coolingDownTimer = 0.0f;
        explodeTargets = new List<Zombie>();
        setPotatoMineState(PotatoMineState.CoolingDown); // 种下即冷却
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        if (state == PlantState.Idle) targetRows = new List<int> { row }; // 只能攻击本行
    }

    protected override void IdleUpdate()
    {
        switch (potatoMineState)
        {
            case PotatoMineState.CoolingDown:
                coolingDownTimer += Time.deltaTime;
                if (coolingDownTimer >= coolingDownTime)
                {
                    setPotatoMineState(PotatoMineState.Recovery);
                    coolingDownTimer = 0.0f;
                }
                break;
            case PotatoMineState.Recovery:
                break;
            case PotatoMineState.Ready:
                if (HaveAttackTarget()) Explode();
                break;
            default:
                break;
        }
    }

    private void setPotatoMineState(PotatoMineState state)
    {
        if (potatoMineState == state) return;
        potatoMineState = state;
        switch (state)
        {
            case PotatoMineState.CoolingDown:
                anim.SetTrigger(AnimatorConfig.plant_coolingDown);
                break;
            case PotatoMineState.Recovery:
                anim.SetTrigger(AnimatorConfig.plant_recovery);
                break;
            case PotatoMineState.Ready:
                anim.SetTrigger(AnimatorConfig.plant_ready);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie && zombie.isBulletHit && !explodeTargets.Contains(zombie)) explodeTargets.Add(zombie);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie) explodeTargets.Remove(zombie);
                break;
            default:
                break;
        }
    }

    protected override bool HaveAttackTarget()
    {
        foreach (Zombie zombie in explodeTargets)
        {
            // 只能被本行触发，有健康目标且能被子弹锁定
            if ((zombie.row == 0 || zombie.row == row) && zombie.isHealthy() && zombie.isBulletHit) return true;
        }
        return false;
    }

    protected override void Explode()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_bomb_potatoMine);
        base.Explode();
    }
}
