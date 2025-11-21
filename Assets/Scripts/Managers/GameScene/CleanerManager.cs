using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CleanerManager : MonoBehaviour
{
    public List<Cleaner> cleaners;
    public List<Transform> cleanerPositions_begin;
    public List<Transform> cleanerPositions_end;

    private int readyCompletedCount;
    public List<Tween> readyTweens = new List<Tween>();
    public List<Tween> activeTweens = new List<Tween>();

    public static CleanerManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        readyCompletedCount = 0;
    }

    private void Start()
    {
    }

    public void getMap()
    {
        cleaners = MapManager.Instance.currMap.cleaners;
        foreach (Cleaner cleaner in cleaners)
        {
            cleaner.gameObject.SetActive(true);
            cleaner.setState(CleanerState.Disable);
        }
        cleanerPositions_begin = MapManager.Instance.currMap.cleanerPositions_begin;
        cleanerPositions_end = MapManager.Instance.currMap.cleanerPositions_end;
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
   
                break;
            case GameState.Previewing:
                foreach (Cleaner cleaner in cleaners) cleaner.gameObject.SetActive(false);
                break;
            case GameState.SelectingCard:

                break;
            case GameState.Ready:
                for (int i = 0; i < cleaners.Count; i++)
                {
                    cleaners[i].gameObject.SetActive(true);
                    cleaners[i].setState(CleanerState.Disable);
                    Tween tween = cleaners[i].transform.DOMove(cleanerPositions_begin[i].position, 0.2f)
                        .SetEase(Ease.InSine)
                        .SetDelay(0.05f * i) // ясЁы
                        .OnComplete(() => CheckTweenCompleted());
                    readyTweens.Add(tween);
                }
                break;
            case GameState.Processing:
                foreach (Cleaner cleaner in cleaners)
                {
                    cleaner.c2d.enabled = true;
                    if (cleaner.state == CleanerState.Enable) cleaner.anim.enabled = true;
                }
                foreach (Tween tween in activeTweens) if (tween != null && tween.IsActive()) tween.Play();
                break;
            case GameState.Paused:
                foreach (Cleaner cleaner in cleaners) cleaner.anim.enabled = false;
                foreach (Tween tween in activeTweens) if (tween != null && tween.IsActive()) tween.Pause();
                break;
            case GameState.Losing:
                foreach (Cleaner cleaner in cleaners)
                {
                    cleaner.c2d.enabled = false;
                    cleaner.anim.enabled = false;
                }
                foreach (Tween tween in activeTweens) if (tween != null && tween.IsActive()) tween.Pause();
                break;
            case GameState.Winning:

                break;
            default:
                break;
        }
    }

    public void CheckTweenCompleted()
    {
        readyCompletedCount++;
        if (readyCompletedCount >= readyTweens.Count) UIManager.Instance.playReadytoPlay();
    }
}
