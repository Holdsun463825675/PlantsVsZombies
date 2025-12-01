using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletState
{
    ToBeUsed,
    Used
}

public enum BulletHitSound
{
    None,
    Kernelpult,
    Butter,
    Melon
}

public class Bullet : Product
{
    private int attackPoint;
    public float speed;
    protected int targetNum; // 最多可攻击的目标（-1为无限）
    public Effect BulletHitPrefab;

    public BulletHitSound hitSound;
    public int hitSoundPriority;

    protected BulletState state;

    private Transform shadow;
    private SpriteRenderer sr;
    private Collider2D c2d;

    protected Zombie targetZombie;
    protected Armor2 targetArmor2;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = 20;
        speed = 5.0f;
        targetNum = 1;
        hitSound = BulletHitSound.None;
        hitSoundPriority = 0;

        shadow = transform.Find("Shadow");
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        Transform child = transform.Find("PeaBulletHit");
        setState(BulletState.ToBeUsed);
    }

    // 暂停继续功能
    public override void Pause()
    {
        transform.DOPause();
    }

    public override void Continue()
    {
        transform.DOPlay();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetNum == 0) return; // 攻击次数用完
        switch (collision.tag)
        {
            case TagConfig.armor2: // 先判定防具
                if (targetArmor2 == collision.GetComponent<Armor2>()) return; // 不攻击重复目标
                targetArmor2 = collision.GetComponent<Armor2>();
                AttackArmor2();
                if (targetNum == 0) setState(BulletState.Used);
                break;
            case TagConfig.zombie:
                if (targetZombie == collision.GetComponent<Zombie>()) return; // 不攻击重复目标
                targetZombie = collision.GetComponent<Zombie>();
                AttackZombie();
                if (targetNum == 0) setState(BulletState.Used);
                break;
            default:
                break;
        }
    }

    protected virtual void AttackZombie()
    {
        if (!targetZombie) return;
        if (targetNum > 0) targetNum--;
        targetZombie.UnderAttack(attackPoint);
        AudioManager.Instance.playHitClip(this, targetZombie);
        //生成特效
        if (BulletHitPrefab) GameObject.Instantiate(BulletHitPrefab, transform.position, Quaternion.identity);
    }

    protected virtual void AttackArmor2()
    {
        if (!targetArmor2) return;
        if (targetNum > 0) targetNum--;
        targetArmor2.UnderAttack(attackPoint);
        AudioManager.Instance.playHitClip(this, targetArmor2);
        //生成特效
        if (BulletHitPrefab) GameObject.Instantiate(BulletHitPrefab, transform.position, Quaternion.identity);
    }

    public void setState(BulletState state)
    {
        if (this.state == state) return;
        this.state = state;
        if (state == BulletState.ToBeUsed)
        {
            c2d.enabled = true;
            if (shadow) shadow.gameObject.SetActive(true);
            targetZombie = null;
        }
        if (state == BulletState.Used)
        {
            transform.DOKill();
            c2d.enabled = false;
            if (shadow) shadow.gameObject.SetActive(false);
            sr.enabled = false;
            ProductManager.Instance.removeProduct(this);
            Destroy(gameObject);
        }
    }

    public void moveToPlace(Vector3 position, float speed = 5.0f)
    {
        transform.DOMove(position, Vector3.Distance(transform.position, position) / speed)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                ProductManager.Instance.removeProduct(this);
                Destroy(gameObject);
            });
    }
}
