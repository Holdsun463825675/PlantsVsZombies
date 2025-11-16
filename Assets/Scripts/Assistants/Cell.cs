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

    public bool PlantPlant(Plant plant)
    {
        if (plant == null) return false;
        if (!PlantAvailable(plant.type)) return false;
        setFlag(plant.type, true);
        plant.transform.position = this.transform.position;
        return true;
    }
}
