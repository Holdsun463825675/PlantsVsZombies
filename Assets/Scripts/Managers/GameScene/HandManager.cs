using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public Plant currPlant = null;
    private Card currCard = null;
    private Shovel currShovel = null;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // 检测鼠标右键点击，0=左键, 1=右键, 2=中键
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlant();
            CancelShovel();
        } 
        FollowCursor();
    }

    private void DestroyCurrPlant()
    {
        if (currPlant != null)
        {
            Destroy(currPlant.gameObject);
            currPlant = null;
        }
    }

    public void CancelPlant()
    {
        DestroyCurrPlant();
        if (currCard)
        {
            currCard.setState(CardState.Ready);
            currCard = null;
        }
    }

    public void SuspendPlant(Card card, PlantID plantID)
    {
        CancelShovel();
        CancelPlant();
        Plant plant = PrefabSystem.Instance.GetPlantPrefab(plantID);
        if (plant == null) return;
        AudioManager.Instance.playClip(ResourceConfig.sound_placeplant_selectcard);
        currPlant = GameObject.Instantiate(plant);
        currPlant.setState(PlantState.Suspension);
        currCard = card;
        currCard.setState(CardState.Selected);
    }

    public void PlantPlant(Cell cell)
    {
        if (!cell.PlantPlant(currPlant)) return;
        currPlant.setCell(cell);
        currPlant.setState(PlantState.Idle);
        currPlant = null;
        AudioManager.Instance.playClip(ResourceConfig.sound_placeplant_plant);
        if (currCard)
        {
            if (GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Conveyor) // 传送带关种植后销毁卡片
            {
                CardManager.Instance.removeCard(currCard);
                Destroy(currCard.gameObject);
                currCard = null;
                return;
            }
            // 其他关卡种植
            SunManager.Instance.AddSun(-currCard.cost);
            currCard.setState(CardState.CoolingDown);
            currCard = null;
        }
    }

    public void SuspendShovel(Shovel shovel)
    {
        CancelPlant();
        currShovel = shovel;
    }

    public void CancelShovel()
    {
        if (currShovel)
        {
            currShovel.setState(ShovelState.TobeUsed);
            currShovel = null;
        }
    }

    void FollowCursor()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousePosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y - 0.5f, 0);
        if (currPlant) currPlant.transform.position = mousePosition;
        if (currShovel) currShovel.transform.position = mousePosition;
    }
}
