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

    private PeaState state;

    private Transform shadow;
    private SpriteRenderer sr;
    private Animator peaBulletHitAnim;
    private Collider2D c2d;

    private Zombie target;

    protected override void Awake()
    {
        base.Awake();
        shadow = transform.Find("Shadow");
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        Transform child = transform.Find("PeaBulletHit");
        if (child) peaBulletHitAnim = child.GetComponent<Animator>();
        if (peaBulletHitAnim)
        {
            peaBulletHitAnim.enabled = false;
            peaBulletHitAnim.gameObject.SetActive(false);
        }
        setState(PeaState.ToBeUsed);
    }

    // ÔÝÍ£¼ÌÐø¹¦ÄÜ
    public override void Pause()
    {
        transform.DOPause();
        if (state == PeaState.Used && peaBulletHitAnim) peaBulletHitAnim.enabled = false;
    }

    public override void Continue()
    {
        transform.DOPlay();
        if (state == PeaState.Used && peaBulletHitAnim) peaBulletHitAnim.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            target = collision.GetComponent<Zombie>();
            setState(PeaState.Used);
        }
    }

    public void setState(PeaState state)
    {
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
            if (peaBulletHitAnim)
            {
                peaBulletHitAnim.gameObject.SetActive(true);
                peaBulletHitAnim.enabled = true;
            }
            if (target)
            {
                target.UnderAttack(attackPoint);
                int idx = Random.Range(0, ResourceConfig.sound_bullethit_splats.Length);
                AudioManager.Instance.playClip(ResourceConfig.sound_bullethit_splats[idx]);
            }
            ProductManager.Instance.removeProduct(this);
            Destroy(gameObject, 1);
        }
        this.state = state;
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
