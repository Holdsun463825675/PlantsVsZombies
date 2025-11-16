using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PeaState
{
    ToBeUsed,
    Used
}

public class Pea : Property
{
    private int attackPoint = 20;
    private float x_speed, x_accelerated_speed, y_speed, y_accelerated_speed;
    private float target_min_x, target_max_x;

    private PeaState state;

    private Transform shadow;
    private SpriteRenderer sr;
    private Animator peaBulletHitAnim;
    private Collider2D c2d;

    private Zombie target;

    protected override void Awake()
    {
        target_min_x = MapManager.Instance.currMap.endlinePositions[0].position.x;
        target_max_x = MapManager.Instance.currMap.endlinePositions[1].position.x;
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

    private void FixedUpdate()
    {
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        switch (state)
        {
            case PeaState.ToBeUsed:
                ToBeUsedUpdate(); 
                break;
            case PeaState.Used:
                UsedUpdate(); 
                break;
            default:
                break;
        }
    }

    // ÔÝÍ£¼ÌÐø¹¦ÄÜ
    public override void Pause()
    {
        if (state == PeaState.Used && peaBulletHitAnim) peaBulletHitAnim.enabled = false;
    }

    public override void Continue()
    {
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
            setSpeed();
        }
        if (state == PeaState.Used)
        {
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
            Destroy(gameObject, 1);
        }
        this.state = state;
    }

    public void setSpeed(float x_speed=5.0f, float x_accelerated_speed=0.0f, float y_speed=0.0f, float y_accelerated_speed=0.0f)
    {
        this.x_speed = x_speed;
        this.x_accelerated_speed = x_accelerated_speed;
        this.y_speed = y_speed;
        this.y_accelerated_speed = y_accelerated_speed;
    }

    private void kinematicsUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.x += x_speed * Time.fixedDeltaTime;
        newPosition.y += y_speed * Time.fixedDeltaTime;
        transform.position = newPosition;

        x_speed += x_accelerated_speed * Time.fixedDeltaTime;
        y_speed += y_accelerated_speed * Time.fixedDeltaTime;
    }

    private void ToBeUsedUpdate()
    {
        kinematicsUpdate();
        if (transform.position.x <= target_min_x || transform.position.x >= target_max_x) Destroy(gameObject);
    }

    private void UsedUpdate()
    {

    }
}
