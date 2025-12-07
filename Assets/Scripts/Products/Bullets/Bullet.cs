using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletState
{
    None, ToBeUsed, Used
}

public enum BulletID
{
    None, Pea, SnowyPea, FirePea,
}

public enum BulletHitSound
{
    None, Kernelpult, Butter, Melon, FirePea, Bowling,
}

public enum BulletType
{
    None, Shoot, Cast
}

public enum BulletDirection
{
    None, Left, Right
}

public class Bullet : Product
{
    public BulletID id;
    public BulletType bulletType;
    public BulletDirection bulletDirection;
    protected BulletID igniteID; // 点燃后的子弹
    protected int attackPoint;
    public List<int> targetRows; // 可攻击的行，0为任意，大于0为行数
    protected List<int> sputterRows; // 可溅射的行
    public float speed;
    public Vector3 target_position; // 目标位置
    protected int targetNum; // 最多可攻击的目标（-1为无限）
    protected bool sputter; // 是否溅射
    public Effect BulletHitPrefab;

    public BulletHitSound hitSound;
    public int hitSoundPriority;

    protected BulletState state;

    private Transform shadow;
    private SpriteRenderer sr;
    private Collider2D c2d;
    private Collider2D sputterC2d;

    protected Zombie targetZombie;
    protected Armor2 targetArmor2;
    protected List<Zombie> sputterTargetZombie;
    protected List<Armor2> sputterTargetArmor2;

    protected override void Awake()
    {
        base.Awake();
        bulletType = BulletType.None;
        bulletDirection = BulletDirection.None;
        attackPoint = 20;
        targetRows = new List<int>();
        speed = 5.0f;
        targetNum = 1;
        sputter = false;
        hitSound = BulletHitSound.None;
        hitSoundPriority = 0;

        shadow = transform.Find("Shadow");
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        Transform child = transform.Find("SputterPlace");
        if (child) sputterC2d = child.GetComponent<Collider2D>();
        if (sputterC2d)
        {
            sputterC2d.enabled = false;
            sputterC2d.GetComponent<TriggerForwarder>().SetBulletParentHandler(this);
        } 

        sputterTargetZombie = new List<Zombie>();
        sputterTargetArmor2 = new List<Armor2>();

        setState(BulletState.ToBeUsed);
    }

    protected virtual void Update()
    {
        if (targetNum == 0) setState(BulletState.Used);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetNum == 0) return; // 攻击次数用完
        switch (collision.tag)
        {
            case TagConfig.armor2: // 先判定防具
                targetArmor2 = collision.GetComponent<Armor2>();
                AttackArmor2();
                break;
            case TagConfig.zombie:
                targetZombie = collision.GetComponent<Zombie>();
                AttackZombie();
                break;
            default:
                break;
        }
    }

    // 父物体处理触发事件的方法
    public virtual void OnChildTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                targetZombie = collision.GetComponent<Zombie>();
                if (targetZombie && !sputterTargetZombie.Contains(targetZombie)) sputterTargetZombie.Add(targetZombie);
                break;
            case TagConfig.armor2:
                targetArmor2 = collision.GetComponent<Armor2>();
                if (targetArmor2 && !sputterTargetArmor2.Contains(targetArmor2)) sputterTargetArmor2.Add(targetArmor2);
                break;
            default:
                break;
        }
    }

    public virtual void OnChildTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.zombie:
                targetZombie = collision.GetComponent<Zombie>();
                if (targetZombie && sputterTargetZombie.Contains(targetZombie)) sputterTargetZombie.Remove(targetZombie);
                break;
            case TagConfig.armor2:
                targetArmor2 = collision.GetComponent<Armor2>();
                if (targetArmor2 && sputterTargetArmor2.Contains(targetArmor2)) sputterTargetArmor2.Remove(targetArmor2);
                break;
            default:
                break;
        }
    }

    public void setBulletType(BulletType type)
    {
        bulletType = type;
    }

    public void setBulletDirection(BulletDirection direction)
    {
        bulletDirection = direction;
    }

    protected virtual void AttackZombie()
    {
        if (!targetZombie || targetNum == 0 || !CanAttack(targetZombie)) return;
        if (targetArmor2 && targetArmor2.zombie == targetZombie) // 优先攻击防具
        {
            AttackArmor2(); return;
        }
        if (!targetZombie.isBulletHit) return; // 不能被子弹造成伤害
        if (targetNum > 0) targetNum--;
        targetZombie.UnderAttack(attackPoint);
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetZombie.underAttackSound, targetZombie.underAttackSoundPriority);
        if (sputter) Sputter(); // 造成溅射伤害
        if (BulletHitPrefab) GameObject.Instantiate(BulletHitPrefab, transform.position, Quaternion.identity);
    }

    protected virtual void AttackArmor2()
    {
        if (!targetArmor2 || targetNum == 0 || !CanAttack(targetArmor2)) return;
        if (targetNum > 0) targetNum--;
        targetArmor2.UnderAttack(attackPoint);
        AudioManager.Instance.playHitClip(hitSound, hitSoundPriority, targetArmor2.underAttackSound, targetArmor2.underAttackSoundPriority);
        targetArmor2 = null; // 攻击防具后清空防具目标
        if (BulletHitPrefab) GameObject.Instantiate(BulletHitPrefab, transform.position, Quaternion.identity);
    }

    protected virtual void Sputter() // 溅射伤害根据目标数量进行衰减
    {
        if (!sputter || sputterTargetZombie.Count + sputterTargetArmor2.Count - 1 == 0) return;
        int totalAttackPoint = attackPoint / 2;
        int point = Mathf.Max(1, totalAttackPoint / (sputterTargetZombie.Count + sputterTargetArmor2.Count - 1)); // 溅射伤害至少1点
        foreach (Zombie zombie in sputterTargetZombie) if (zombie != targetZombie && CanSputter(zombie)) zombie.UnderAttack(point);
        foreach (Armor2 armor2 in sputterTargetArmor2) if (CanSputter(armor2)) armor2.UnderAttack(point);
    }

    public void setState(BulletState state)
    {
        if (this.state == state) return;
        this.state = state;
        if (state == BulletState.ToBeUsed)
        {
            c2d.enabled = true;
            if (sputterC2d) sputterC2d.enabled = true;
            if (shadow) shadow.gameObject.SetActive(true);
            targetZombie = null;
            targetArmor2 = null;
        }
        if (state == BulletState.Used)
        {
            c2d.enabled = false;
            if (sputterC2d) sputterC2d.enabled = false;
            transform.DOKill();
            if (shadow) shadow.gameObject.SetActive(false);
            sr.enabled = false;
            ProductManager.Instance.removeProduct(this);
            Destroy(gameObject);
        }
    }

    public virtual void setTargetRows(List<int> targetRows)
    {
        this.targetRows = targetRows;
        sputterRows = targetRows;
    }

    public void moveToPlace(Vector3 position, float speed = 5.0f)
    {
        this.speed = speed;
        target_position = position;
        transform.DOMove(position, this.speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => setState(BulletState.Used));
    }

    public virtual Bullet Ignite()
    {
        Bullet bulletPrefab = null;
        foreach (Bullet bullet in PrefabSystem.Instance.bulletPrefabs)
        {
            if (bullet.id == igniteID) bulletPrefab = bullet;
        }
        if (!bulletPrefab) return null;
        AudioManager.Instance.playClip(ResourceConfig.sound_fire_firepea); // 点燃音效
        Bullet newBullet = GameObject.Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.setBulletType(bulletType); newBullet.setBulletDirection(bulletDirection); newBullet.setTargetRows(targetRows);
        newBullet.setState(BulletState.ToBeUsed);
        newBullet.moveToPlace(target_position);
        setState(BulletState.Used);
        return newBullet;
    }

    protected virtual bool CanAttack(Zombie zombie)
    {
        return (zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row)) && zombie.isBulletHit;
    }

    protected virtual bool CanAttack(Armor2 armor2)
    {
        return armor2.zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(armor2.zombie.row);
    }

    protected virtual bool CanSputter(Zombie zombie)
    {
        return (zombie.row == 0 || sputterRows.Contains(0) || sputterRows.Contains(zombie.row)) && zombie.isBulletHit;
    }

    protected virtual bool CanSputter(Armor2 armor2)
    {
        return armor2.zombie.row == 0 || sputterRows.Contains(0) || sputterRows.Contains(armor2.zombie.row);
    }
}
