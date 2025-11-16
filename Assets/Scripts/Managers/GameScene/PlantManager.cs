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

    // ÔÝÍ£¼ÌÐø
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
}
