using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cell : MonoBehaviour, IClickable
{
    private int rowMaxSortingOrder = 5000;
    public int row;
    public Dictionary<PlantType, List<Plant>> plants = new Dictionary<PlantType, List<Plant>>();
    private Plant virtualPlant;


    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30000;
        priority.isClickable = true;
    }

    private void Update()
    {
        if (!HandManager.Instance.currPlant) unsetVirtualPlant();
    }

    private void OnMouseEnter()
    {
        setVirtualPlant(HandManager.Instance.currPlant);
    }

    private void OnMouseExit()
    {
        unsetVirtualPlant();
    }

    public void OnClick()
    {
        HandManager.Instance.PlantPlant(this);
    }

    public void addPlant(Plant plant)
    {
        if (!plants.ContainsKey(plant.type)) plants.Add(plant.type, new List<Plant>());
        if (!plants[plant.type].Contains(plant)) plants[plant.type].Add(plant);
    }

    public void removePlant(Plant plant)
    {
        if (!plants.ContainsKey(plant.type)) return;
        plants[plant.type].Remove(plant);
    }

    private Plant getPlant(PlantID id)
    {
        foreach (PlantType plantType in plants.Keys)
        {
            foreach (Plant pl in plants[plantType])
            {
                if (pl.id == id) return pl;
            }
        }
        return null;
    }

    // 判断是否可种植植物
    public bool PlantAvailable(Plant plant)
    {
        if (plant.prePlantID == PlantID.None) // 不需要前置植物
        {
            if (plants.ContainsKey(plant.type) && plants[plant.type].Count > 0) return false;
        }
        else // 需要前置植物
        {
            Plant prePlant = getPlant(plant.prePlantID);
            if (prePlant == null) return false;
        }
        return true;
    }

    private Vector3 plantPlace(Plant plant)
    {
        float target_y = transform.position.y;
        switch (plant.type)
        {
            case PlantType.Carrier:
                target_y += 0.1f;
                break;
            case PlantType.Surrounding:
                target_y += 0.2f;
                break;
            case PlantType.Normal:
                target_y += 0.3f;
                break;
            case PlantType.Flight:
                target_y += 0.8f;
                break;
            default:
                break;
        }
        return new Vector3(transform.position.x, target_y, transform.position.z);
    }

    private int getPlantSortingOrder(Plant plant)
    {
        int res = row * rowMaxSortingOrder;
        switch (plant.type)
        {
            case PlantType.Carrier:
                res += 4;
                break;
            case PlantType.Surrounding:
                res += 12;
                break;
            case PlantType.Normal:
                res += 8;
                break;
            case PlantType.Flight:
                res += 16;
                break;
            default:
                break;
        }
        return res;
    }

    private void setVirtualPlant(Plant plant)
    {
        if (plant == null) return;
        if (!PlantAvailable(plant)) return;
        Plant virtualPlantPrefab = PrefabSystem.Instance.GetPlantPrefab(plant.id);
        virtualPlant = GameObject.Instantiate(virtualPlantPrefab);
        virtualPlant.setSortingOrder(virtualPlant.GetComponent<SpriteRenderer>().sortingOrder - 1);
        virtualPlant.transform.position = plantPlace(virtualPlant);
        // 调整透明度
        Color currentColor = virtualPlant.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 127f / 255f);
        virtualPlant.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void unsetVirtualPlant()
    {
        if (virtualPlant)
        {
            Destroy(virtualPlant.gameObject);
            virtualPlant = null;
        }
    }

    public bool PlantPlant(Plant plant)
    {
        if (plant == null) return false;
        if (!PlantAvailable(plant)) return false;

        if (plant.prePlantID != PlantID.None) // 需要前置植物
        {
            getPlant(plant.prePlantID).setState(PlantState.Die); // 替换前置植物
        }

        addPlant(plant);
        plant.setSortingOrder(getPlantSortingOrder(plant));
        plant.transform.position = plantPlace(plant);
        return true;
    }
}
