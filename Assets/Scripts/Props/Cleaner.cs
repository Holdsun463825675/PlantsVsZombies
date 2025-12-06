using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CleanerState
{
    None,
    Disable,
    Enable
}

public class Cleaner : MonoBehaviour
{
    private int dieMode = 3;
    public CleanerState state;
    public Animator anim;
    public BoxCollider2D c2d;

    public int row;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        c2d = GetComponent<BoxCollider2D>();
        setState(CleanerState.Disable);
    }

    public void setState(CleanerState state)
    {
        if (this.state == state) return;
        switch (state)
        {
            case CleanerState.Disable:
                anim.enabled = false;
                c2d.enabled = false;
                break;
            case CleanerState.Enable:
                c2d.enabled = true;
                ZombieManager.Instance.spawnProtection[row] += 3; // 出怪保护
                AudioManager.Instance.playClip(ResourceConfig.sound_other_lawnmower);
                Tween tween = transform.DOMove(CleanerManager.Instance.cleanerPositions_end[row].position, 3f)
                        .SetEase(Ease.Linear)
                        .OnComplete(() =>{
                            CleanerManager.Instance.cleaners.Remove(this);
                            Destroy(gameObject); 
                        });
                CleanerManager.Instance.activeTweens.Add(tween);
                if (GameManager.Instance.state == GameState.Paused) tween.Pause();
                else anim.enabled = true;
                break;
        }
        this.state = state;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            // 僵尸不掉头时触发
            if (zombie.isHealthy()) setState(CleanerState.Enable);
            if (state == CleanerState.Enable) zombie.kill(dieMode); // 生效时机制杀
        }
    }

    private void OnMouseDown()
    {
        setState(CleanerState.Enable);
    }
}
