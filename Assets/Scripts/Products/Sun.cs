using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum SunState
{
    Vertical,
    ProducedbySunflower,
    Stop,
    Collected
}

public class Sun : Product, IClickable
{
    public SunState state;

    private int value = 25;

    private float survivalTime, autoCollectedTime, collectedTime;
    private float survivalTimer, autoCollectedTimer;

    private float verticalSpeed = 1.0f;
    private float producedbySunflowerMoveTime = 1.0f;
    private float target_y;

    protected override void Awake()
    {
        base.Awake();
        state = SunState.Stop;
        survivalTime = 30.0f; autoCollectedTime = 1.0f; collectedTime = 0.5f;
        survivalTimer = 0.0f; autoCollectedTimer = 0.0f;
        target_y = 0.0f;
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30002;
        priority.isClickable = true;
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        // 自动收集
        autoCollectedTimer += Time.deltaTime;
        if (SettingSystem.Instance.settingsData.autoCollected && 
            autoCollectedTimer > autoCollectedTime / Time.timeScale) setState(SunState.Collected); // TODO: 玄学

        if (state == SunState.Stop)
        {
            survivalTimer += Time.deltaTime;
            if (survivalTimer >= survivalTime)
            {
                ProductManager.Instance.removeProduct(this);
                Destroy(gameObject);
            }
        }
    }

    public void setTargetY(float y)
    {
        target_y = y;
    }

    public void setState(SunState state)
    {
        if (this.state == state) return;
        this.state = state;
        switch (state)
        {
            case SunState.Vertical:
                transform.DOMove(new Vector2(transform.position.x, target_y), verticalSpeed)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .OnComplete(() => setState(SunState.Stop));
                break;
            case SunState.ProducedbySunflower:
                // 计算中间控制点（抛物线顶点）
                Vector3 startPosition = transform.position;
                float target_x = transform.position.x + Random.Range(0.5f, 1.0f) * (Random.Range(0, 2) * 2 - 1);
                Vector3 endPosition = new Vector3(target_x, target_y, transform.position.z);
                Vector3 midPoint = (startPosition + endPosition) * 0.5f;
                midPoint.y += Random.Range(0.5f, 0.75f);

                // 创建路径点数组
                Vector3[] path = new Vector3[] { startPosition, midPoint, endPosition };

                // 使用DOPath进行抛物线移动
                transform.DOPath(path, producedbySunflowerMoveTime, PathType.CatmullRom)
                      .SetEase(Ease.InOutSine)
                      .OnComplete(() => setState(SunState.Stop));
                break;
            case SunState.Stop:
                break;
            case SunState.Collected:
                AudioManager.Instance.playClip(ResourceConfig.sound_collectitem_sun);
                Vector2 collectedPosition = SunManager.Instance.collectedTarget.position;

                // 消除当前所有移动
                transform.DOKill();
                transform.DOMove(SunManager.Instance.collectedTarget.position, collectedTime)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => {
                        SunManager.Instance.AddSun(value);
                        ProductManager.Instance.removeProduct(this);
                        Destroy(gameObject);
                    });

                if (GameManager.Instance.state != GameState.Paused) anim.enabled = true;
                else transform.DOPause();
                GetComponent<CircleCollider2D>().enabled = false;
                break;
        }
    }

    public void OnClick()
    {    
        setState(SunState.Collected);
    }
}
