using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DefensePlantHealthState
{
    None,
    Healthy,
    Middling,
    Unwell,
    Die,
}

public class DefensePlant : Plant
{
    protected DefensePlantHealthState defenseHealthState;
    protected float middlingHealthPercentage, unwellHealthPercentage, dieHealthPercentage;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = 4000; currHealth = maxHealth;
        middlingHealthPercentage = 0.666f; unwellHealthPercentage = 0.333f; dieHealthPercentage = 1e-10f;
        underAttackSound = 1;
        setDefenseHealthState(DefensePlantHealthState.Healthy);
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        float healthPercentage = (float)currHealth / (float)maxHealth;
        anim.SetFloat(AnimatorConfig.plant_HealthPercentage, healthPercentage);
        if (healthPercentage >= middlingHealthPercentage) setDefenseHealthState(DefensePlantHealthState.Healthy);
        else if (healthPercentage >= unwellHealthPercentage && healthPercentage < middlingHealthPercentage) setDefenseHealthState(DefensePlantHealthState.Middling);
        else if (healthPercentage >= dieHealthPercentage && healthPercentage < unwellHealthPercentage) setDefenseHealthState(DefensePlantHealthState.Unwell);
        else if (healthPercentage < dieHealthPercentage) setDefenseHealthState(DefensePlantHealthState.Die);
    }

    protected virtual void setDefenseHealthState(DefensePlantHealthState state)
    {
        if (defenseHealthState == state) return;
        defenseHealthState = state;
    }
}
