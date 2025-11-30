using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance { get; private set; }

    private List<Plant> plantList = new List<Plant>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (HandManager.Instance.currPlant) setPlantTwinkle(HandManager.Instance.currPlant.prePlantID); // 要种植的前置植物闪烁
        else setPlantTwinkle(PlantID.None);
    }

    // 暂停继续
    public void Pause()
    {
        foreach (Plant plant in plantList) if (plant) plant.Pause();
    }

    public void Continue()
    {
        foreach (Plant plant in plantList) if (plant) plant.Continue();
    }

    public void addPlant(Plant plant)
    {
        plantList.Add(plant);
    }

    public void removePlant(Plant plant)
    {
        plantList.Remove(plant);
    }

    private void setPlantTwinkle(PlantID id)
    {
        foreach (Plant plant in plantList)
        {
            if (plant)
            {
                if (plant.id == id) plant.anim.SetBool(AnimatorConfig.plant_twinkle, true);
                else plant.anim.SetBool(AnimatorConfig.plant_twinkle, false);
            }
        }
    }
}
