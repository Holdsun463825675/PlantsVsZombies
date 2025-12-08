using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public GameObject UI;
    public Canvas cardUI;
    private List<Transform> UIPlaces;

    private float previewPauseTime = 1.5f;
    private float previewMoveTime = 1.5f;
    private float readyMoveTime = 1.5f;
    private float losingMoveTime = 1.5f;
    private Transform beginPlace, endPlace, losingPlace;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public void getMap()
    {
        UIPlaces = MapManager.Instance.currMap.cameraPositions;
        beginPlace = UIPlaces[0]; endPlace = UIPlaces[1]; losingPlace = UIPlaces[2];
        UI.transform.position = losingPlace.position;
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
                UI.transform.position = losingPlace.position;
                break;
            case GameState.Previewing:
                cardUI.sortingLayerName = "Foreground";
                previewingPause();
                break;
            case GameState.SelectingCard:
                break;
            case GameState.Ready:
                cardUI.sortingLayerName = "CardList";
                readyMove();
                break;
            case GameState.Processing:
                CellManager.Instance.setState(GameState.Ready);
                if (GameManager.Instance.currLevelConfig.cleaner) CleanerManager.Instance.setState(GameState.Ready);
                else UIManager.Instance.playReadytoPlay();
                break;
            case GameState.Losing:
                losingMove();
                break;
            default:
                break;
        }
    }

    private void previewingPause()
    {
        UI.transform.DOMove(losingPlace.position, previewPauseTime).OnComplete(() => previewingMove());
    }

    private void previewingMove()
    {
        UI.transform.DOMove(endPlace.position, previewMoveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                GameManager.Instance.setState(GameState.SelectingCard);
            });
    }

    private void readyMove()
    {
        UI.transform.DOMove(beginPlace.position, readyMoveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => setState(GameState.Processing));
    }

    private void losingMove()
    {
        UI.transform.DOMove(losingPlace.position, losingMoveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => UIManager.Instance.setState(GameState.Losing));
    }
}
