using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour
{
    public static GameSpeedManager Instance { get; private set; }

    private float currentSpeed = 1f;
    private Dictionary<Animator, float> originalAnimatorSpeeds = new Dictionary<Animator, float>();

    void Awake()
    {
        Instance = this;
    }

    public void SetGameSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0f, 5f);

        // 1. 设置Unity时间系统
        Time.timeScale = currentSpeed;
        Time.fixedDeltaTime = 0.02f * currentSpeed;

        // 2. 设置DOTween全局时间缩放
        DOTween.timeScale = currentSpeed;

        // 3. 更新所有Animator
        UpdateAllAnimators();

        Debug.Log($"游戏速度设置为: {currentSpeed}x");
    }

    private void UpdateAllAnimators()
    {
        var allAnimators = FindObjectsOfType<Animator>();
        foreach (var animator in allAnimators)
        {
            if (!originalAnimatorSpeeds.ContainsKey(animator))
            {
                originalAnimatorSpeeds[animator] = animator.speed;
            }

            animator.speed = originalAnimatorSpeeds[animator] * currentSpeed;
        }
    }

    // 为新创建的Animator注册
    public void RegisterAnimator(Animator animator)
    {
        if (animator != null && !originalAnimatorSpeeds.ContainsKey(animator))
        {
            originalAnimatorSpeeds[animator] = animator.speed;
            animator.speed = originalAnimatorSpeeds[animator] * currentSpeed;
        }
    }

    // 清理销毁的Animator
    public void UnregisterAnimator(Animator animator)
    {
        if (animator != null)
        {
            originalAnimatorSpeeds.Remove(animator);
        }
    }
}