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

    private void FixedUpdate()
    {
        switch (state)
        {
            case CleanerState.Disable:
                DisableUpdate();
                break;
            case CleanerState.Enable:
                EnableUpdate();
                break;
        }
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
                ZombieManager.Instance.spawnProtection[row] += 3; // ³ö¹Ö±£»¤
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

    private void DisableUpdate()
    {

    }

    private void EnableUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            setState(CleanerState.Enable);
            Zombie zombie = collision.GetComponent<Zombie>();
            zombie.kill(dieMode);
        }
    }

    private void OnMouseDown()
    {
        setState(CleanerState.Enable);
    }
}
