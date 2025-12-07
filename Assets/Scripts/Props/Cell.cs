using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CellType
{
    None = 0, Grass = 1, Pool = 2, Roof = 3,
}

public class Cell : MonoBehaviour, IClickable
{
    private int rowMaxSortingOrder = 5000, colMaxSortingOrder = 500;
    public int row, col;
    public CellType cellType = CellType.None;
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
        if (plant.type == PlantType.None) return;
        if (!plants.ContainsKey(plant.type)) plants.Add(plant.type, new List<Plant>());
        if (!plants[plant.type].Contains(plant)) plants[plant.type].Add(plant);
    }

    public void removePlant(Plant plant)
    {
        if (plant.type == PlantType.None) return;
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
        if (cellType == CellType.None) return false;
        if (plant.type == PlantType.None) return true;
        if (plant.prePlantID == PlantID.None) // 不需要前置植物，需判断格子类型
        {
            if (plants.ContainsKey(plant.type) && plants[plant.type].Count > 0) return false;
            if (!plant.cellTypes.Contains(cellType))
            {
                if (!plants.ContainsKey(PlantType.Carrier) || plants[PlantType.Carrier].Count == 0) return false;
            }
        }
        else // 需要前置植物，只需判断前置植物
        {
            Plant prePlant = getPlant(plant.prePlantID);
            if (prePlant == null) return false;
        }
        return true;
    }

    private Vector3 plantPlace(Plant plant)
    {
        float baseY = 0.0f, offset = 0.0f, carrier_offset = 0.0f;
        // 有载体时抬高
        if (plants.ContainsKey(PlantType.Carrier) && plants[PlantType.Carrier].Count > 0)
        {
            switch (cellType)
            {
                case CellType.Grass:
                    carrier_offset = 0.3f;
                    break;
                case CellType.Pool:
                    carrier_offset = 0.15f;
                    break;
                case CellType.Roof:
                    baseY = 0.2f; carrier_offset = 0.3f;
                    break;
                default:
                    break;
            }
        }
        switch (plant.type)
        {
            case PlantType.Normal:
                offset = 0.2f;
                break;
            case PlantType.Carrier:
                offset = 0.0f;
                break;
            case PlantType.Surrounding:
                offset = 0.0f;
                break;
            case PlantType.Flight:
                offset = 0.3f;
                break;
            default:
                break;
        }
        Vector3 newPlace = new Vector3(transform.position.x, transform.position.y + baseY + offset, transform.position.z);
        if (plant.type != PlantType.Carrier) newPlace.y += carrier_offset;
        return newPlace;
    }

    private int getPlantSortingOrder(Plant plant)
    {
        int res = row * rowMaxSortingOrder - col * colMaxSortingOrder;
        switch (plant.type)
        {
            case PlantType.None:
                res += 8;
                break;
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
        adjustPlantPlace();
        return true;
    }

    public void adjustPlantPlace()
    {
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            PlantType plantType = pair.Key;
            foreach (Plant plant in pair.Value) if (plant) plant.transform.position = plantPlace(plant);
        }
    }

    public bool canShovel(Plant plant) // 是否可以铲植物
    {
        if (plant.type != PlantType.Carrier) return true;
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            PlantType plantType = pair.Key;
            if (plantType == PlantType.Carrier) continue;
            foreach (Plant pl in pair.Value) if (!pl.cellTypes.Contains(cellType)) return false;
        }
        return true;
    }
}
