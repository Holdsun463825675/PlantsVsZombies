using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SunState
{
    Vertical,
    ProducedbySunflower,
    Stop,
    Collected
}

public class Sun : MonoBehaviour, IClickable
{
    public SunState state;

    private int value = 25;

    private float survivalTime, collectedTime, autoCollectedTime;
    private float survivalTimer, collectedTimer, autoCollectedTimer;

    private float x_speed, x_accelerated_speed, y_speed, y_accelerated_speed;
    private float target_y;

    private Animator anim;

    private void Awake()
    {
        state = SunState.Stop;
        survivalTime = 30.0f; collectedTime = 0.5f; autoCollectedTime = 1.0f;
        survivalTimer = 0.0f; collectedTimer = 0.0f; autoCollectedTimer = 0.0f;
        x_speed = 0.0f;
        x_accelerated_speed = 0.0f;
        y_speed = 0.0f;
        y_accelerated_speed = 0.0f;
        target_y = 0.0f;
        anim = GetComponent<Animator>();
        SunManager.Instance.addSun(this);
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30001;
        priority.isClickable = true;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        // 自动收集
        autoCollectedTimer += Time.fixedDeltaTime;
        if (GameManager.Instance.autoCollected && autoCollectedTimer > autoCollectedTime) setState(SunState.Collected);

        kinematicsUpdate();
        switch (state)
        {
            case SunState.Vertical:
                VerticalUpdate();
                break;
            case SunState.ProducedbySunflower:
                ProducedbySunflowerUpdate();
                break;
            case SunState.Stop:
                StopUpdate();
                break;
            case SunState.Collected:
                CollectedUpdate();
                break;
            default:
                break;
        }
    }

    // 暂停继续功能
    public void Pause()
    {
        if (state != SunState.Collected) anim.enabled = false;
    }

    public void Continue()
    {
        if (state != SunState.Collected) anim.enabled = true;
    }

    public void setTargetY(float y)
    {
        target_y = y;
    }

    public void setState(SunState state)
    {
        if (this.state == state) return;
        if (state == SunState.Vertical)
        {
            x_speed = 0.0f;
            x_accelerated_speed = 0.0f;
            y_speed = -1.0f;
            y_accelerated_speed = 0.0f;
        }
        if (state == SunState.ProducedbySunflower)
        {
            x_speed = Random.Range(0.25f, 0.75f) * (Random.Range(0, 2) * 2 - 1);
            x_accelerated_speed = 0.0f;
            y_speed = Random.Range(0.75f, 1.25f);
            y_accelerated_speed = -2.0f;
        }
        if (state == SunState.Stop)
        {
            x_speed = 0.0f;
            x_accelerated_speed = 0.0f;
            y_speed = 0.0f;
            y_accelerated_speed = 0.0f;
        }
        if (state == SunState.Collected)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_collectitem_sun);
            if (GameManager.Instance.state != GameState.Paused) anim.enabled = true;
            GetComponent<CircleCollider2D>().enabled = false;
            Vector2 collectedPosition = SunManager.Instance.getSunCollectedPosition();
            float collected_target_x = collectedPosition.x;
            float collected_target_y = collectedPosition.y;
            x_speed = (collected_target_x - transform.position.x) / collectedTime;
            x_accelerated_speed = 0.0f;
            y_speed = (collected_target_y - transform.position.y) / collectedTime;
            y_accelerated_speed = 0.0f;
        }
        this.state = state;
    }

    public void OnClick()
    {    
        setState(SunState.Collected);
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

    private void VerticalUpdate()
    {
        if (transform.position.y < target_y) setState(SunState.Stop);
    }

    private void ProducedbySunflowerUpdate()
    {
        if (transform.position.y < target_y) setState(SunState.Stop);
    }

    private void StopUpdate()
    {
        survivalTimer += Time.fixedDeltaTime;
        if (survivalTimer > survivalTime)
        {
            Destroy(gameObject);
        }
    }

    private void CollectedUpdate()
    {
        collectedTimer += Time.fixedDeltaTime;
        if (collectedTimer > collectedTime)
        {
            SunManager.Instance.AddSun(value);
            Destroy(gameObject);
        }
    }
}
