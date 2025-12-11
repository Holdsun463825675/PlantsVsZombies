using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaredyShroomState
{
    None, Normal, Scared, Grow
}

public class ScaredyShroom : AttackPlant
{
    private float scaredDistance;
    private List<int> scaredRows;
    protected ScaredyShroomState scaredyShroomState;

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.ScaredyShroom;
        type = PlantType.Normal;
        sleepTime = new List<TimeOfGame> { TimeOfGame.Day };
        attackTime = 1.5f;
        attackTimer = Random.Range(0.0f, attackTime);
        bulletID = BulletID.Spore;
        bulletPrefab = PrefabSystem.Instance.GetBulletPrefab(bulletID);
        scaredDistance = 2.25f; scaredRows = new List<int>();
        setScaredyShroomState(ScaredyShroomState.Normal);
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        // 只对本行、上下行的僵尸胆小
        if (state == PlantState.Idle) scaredRows = new List<int> { Mathf.Max(CellManager.Instance.minRow, row - 1), row, Mathf.Min(row + 1, CellManager.Instance.maxRow) };
    }

    protected override void IdleUpdate()
    {
        base.IdleUpdate();
        if (!canAct()) return;
        Zombie nearstZombie = ZombieManager.Instance.getNearestZombie(transform.position, scaredRows);
        if (nearstZombie == null || Vector3.Distance(transform.position, nearstZombie.transform.position) > scaredDistance)
        {
            if (scaredyShroomState == ScaredyShroomState.Scared) setScaredyShroomState(ScaredyShroomState.Grow);
        }
        else setScaredyShroomState(ScaredyShroomState.Scared);
    }

    protected void setScaredyShroomState(ScaredyShroomState state)
    {
        if (scaredyShroomState == state) return;
        scaredyShroomState = state;
        switch (state)
        {
            case ScaredyShroomState.Normal:
                break;
            case ScaredyShroomState.Scared:
                anim.SetTrigger(AnimatorConfig.plant_coolingDown);
                break;
            case ScaredyShroomState.Grow:
                anim.SetTrigger(AnimatorConfig.plant_recovery);
                break;
            default:
                break;
        }
    }

    protected override void setAttack()
    {
        if (scaredyShroomState != ScaredyShroomState.Normal) return; // 只有正常时会攻击
        base.setAttack();
    }


    protected override void Attack()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_puff);
        Bullet bullet = GameObject.Instantiate(bulletPrefab, effectPlace.position, Quaternion.identity);
        bullet.setBulletType(BulletType.Shoot); bullet.setBulletDirection(BulletDirection.Right); bullet.setTargetRows(targetRows);
        bullet.setState(BulletState.ToBeUsed);
        float target_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
        bullet.moveToPlace(new Vector3(target_x, effectPlace.position.y, effectPlace.position.z));
    }
}
