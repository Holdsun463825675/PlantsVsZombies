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
        // ¼ì²âÊó±êÓÒ¼üµã»÷£¬0=×ó¼ü, 1=ÓÒ¼ü, 2=ÖÐ¼ü
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
        Plant plant = PlantManager.Instance.GetPlantPrefab(plantID);
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
        currPlant.setState(PlantState.Idle);
        currPlant.setCell(cell);
        currPlant = null;
        AudioManager.Instance.playClip(ResourceConfig.sound_placeplant_plant);
        if (currCard)
        {
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
