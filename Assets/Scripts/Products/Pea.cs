using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PeaState
{
    ToBeUsed,
    Used
}

public class Pea : Product
{
    private int attackPoint = 20;
    public float speed = 5.0f;
    public Effect peaBulletHitPrefab;

    private PeaState state;

    private Transform shadow;
    private SpriteRenderer sr;
    private Collider2D c2d;

    private Zombie target;

    protected override void Awake()
    {
        base.Awake();
        shadow = transform.Find("Shadow");
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        Transform child = transform.Find("PeaBulletHit");
        setState(PeaState.ToBeUsed);
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
        if (target != null) return; // 保证只攻击一个目标
        if (collision.tag == TagConfig.zombie)
        {
            target = collision.GetComponent<Zombie>();
            setState(PeaState.Used);
        }
    }

    public void setState(PeaState state)
    {
        if (this.state == state) return;
        this.state = state;
        if (state == PeaState.ToBeUsed)
        {
            c2d.enabled = true;
            if (shadow) shadow.gameObject.SetActive(true);
            target = null;
        }
        if (state == PeaState.Used)
        {
            transform.DOKill();
            c2d.enabled = false;
            if (shadow) shadow.gameObject.SetActive(false);
            sr.enabled = false;
            if (target)
            {
                target.UnderAttack(attackPoint);
                AudioManager.Instance.playHitClip(this, target);
                //生成特效
                Debug.Log(transform.position);
                GameObject.Instantiate(peaBulletHitPrefab, transform.position, Quaternion.identity);
            }
            ProductManager.Instance.removeProduct(this);
            Destroy(gameObject);
        }
    }

    public void moveToPlace(Vector3 position, float speed=5.0f)
    {
        transform.DOMove(position, Vector3.Distance(transform.position, position) / speed)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                ProductManager.Instance.removeProduct(this);
                Destroy(gameObject);});
    }
}
