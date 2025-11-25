using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, IClickable
{
    public Dictionary<PlantType, bool> flag = new Dictionary<PlantType, bool>();

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30000;
        priority.isClickable = true;
    }

    public void OnClick()
    {
        HandManager.Instance.PlantPlant(this);
    }

    public void setFlag(PlantType plantType, bool flag)
    {
        this.flag[plantType] = flag;
    }

    // 判断是否可种植植物
    public bool PlantAvailable(PlantType plantType)
    {
        if (flag.ContainsKey(plantType) && flag[plantType]) return false;
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

    public bool PlantPlant(Plant plant)
    {
        if (plant == null) return false;
        if (!PlantAvailable(plant.type)) return false;
        setFlag(plant.type, true);
        plant.transform.position = plantPlace(plant);
        return true;
    }
}
