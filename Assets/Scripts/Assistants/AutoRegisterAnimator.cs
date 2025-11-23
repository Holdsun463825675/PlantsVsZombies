using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoRegisterAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 自动注册到GameSpeedManager
        if (GameSpeedManager.Instance != null && animator != null)
        {
            GameSpeedManager.Instance.RegisterAnimator(animator);
        }
    }

    void OnDestroy()
    {
        // 自动注销
        if (GameSpeedManager.Instance != null && animator != null)
        {
            GameSpeedManager.Instance.UnregisterAnimator(animator);
        }
    }
}