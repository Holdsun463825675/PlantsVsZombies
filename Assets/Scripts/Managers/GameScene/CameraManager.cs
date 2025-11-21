using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public GameObject UI;
    private List<Transform> UIPlaces;

    private float previewPauseTime = 1.5f;
    private float previewPauseTimer;
    private float cameraSpeed = 5.0f;
    private float speed;
    private Transform beginPlace, endPlace, losingPlace;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        switch (GameManager.Instance.state)
        {
            case GameState.NotStarted:
                NotStartedUpdate();
                break;
            case GameState.Previewing:
                PreviewingUpdate();
                break;
            case GameState.SelectingCard:
                SelectingCardUpdate();
                break;
            case GameState.Ready:
                ReadyUpdate();
                break;
            case GameState.Losing:
                LosingUpdate();
                break;
            default:
                GameUpdate();
                break;
        }

    }

    public void getMap()
    {
        UIPlaces = MapManager.Instance.currMap.cameraPositions;
        beginPlace = UIPlaces[0]; endPlace = UIPlaces[1]; losingPlace = UIPlaces[2];
        UI.transform.position = losingPlace.transform.position;
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
                UI.transform.position = beginPlace.position;
                break;
            case GameState.Previewing:
                previewPauseTimer = 0.0f;
                speed = cameraSpeed;
                break;
            case GameState.SelectingCard:
                speed = 0.0f;
                break;
            case GameState.Ready:
                speed = -cameraSpeed;
                break;
            case GameState.Losing:
                speed = -cameraSpeed;
                break;
            default:
                speed = 0.0f;
                break;
        }
    }

    private void kinematicsUpdate()
    {
        Vector3 newPosition = UI.transform.position;
        newPosition.x += speed * Time.fixedDeltaTime;
        UI.transform.position = newPosition;
    }

    private void PreviewingUpdate()
    {
        if (previewPauseTimer >= previewPauseTime)
        {
            kinematicsUpdate();
            if (UI.transform.position.x >= endPlace.position.x) GameManager.Instance.setState(GameState.SelectingCard);
        }
        else previewPauseTimer += Time.fixedDeltaTime;
    }

    private void NotStartedUpdate()
    {

    }

    private void SelectingCardUpdate()
    {

    }

    private void ReadyUpdate()
    {
        kinematicsUpdate();
        if (UI.transform.position.x < beginPlace.position.x)
        {
            Vector3 newPosition = beginPlace.position;
            UI.transform.position = newPosition;

            setState(GameState.Processing);
            // 加入小推车
            if (GameManager.Instance.currLevelConfig.cleaner) CleanerManager.Instance.setState(GameState.Ready);
            else UIManager.Instance.playReadytoPlay();
        }
    }

    private void GameUpdate()
    {

    }

    private void LosingUpdate()
    {
        kinematicsUpdate();
        if (UI.transform.position.x < losingPlace.position.x)
        {
            Vector3 newPosition = losingPlace.position;
            UI.transform.position = newPosition;
            speed = 0.0f;
            UIManager.Instance.setState(GameState.Losing);
        }
    }

}
