using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public List<Plant> plantList;
    private Plant currPlant = null;
    private Card currCard = null;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // ¼ì²âÊó±êÓÒ¼üµã»÷£¬0=×ó¼ü, 1=ÓÒ¼ü, 2=ÖÐ¼ü
        if (Input.GetMouseButtonDown(1)) CancelPlant();
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
        CancelPlant();
        Plant plant = GetPlantPrefab(plantID);
        if (plant == null)
        {
            Debug.Log("Î´ÕÒµ½ÒªÖÖÖ²µÄÖ²Îï");
            return;
        }
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

    private Plant GetPlantPrefab(PlantID plantID)
    {
        foreach (Plant plant in plantList)
        {
            if (plant.id == plantID) return plant;
        }
        return null;
    }

    void FollowCursor()
    {
        if (!currPlant) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        currPlant.transform.position = mouseWorldPosition;
    }
}
