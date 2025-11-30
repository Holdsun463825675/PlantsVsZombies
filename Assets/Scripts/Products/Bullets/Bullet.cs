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

    protected Zombie target;

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
        if (collision.tag == TagConfig.zombie)
        {
            if (target == collision.GetComponent<Zombie>()) return; // 不攻击重复目标
            target = collision.GetComponent<Zombie>();
            Attack();
            if (targetNum == 0) setState(BulletState.Used);
        }
    }

    protected virtual void Attack()
    {
        if (!target) return;
        if (targetNum > 0) targetNum--;
        target.UnderAttack(attackPoint);
        AudioManager.Instance.playHitClip(this, target);
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
            target = null;
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
