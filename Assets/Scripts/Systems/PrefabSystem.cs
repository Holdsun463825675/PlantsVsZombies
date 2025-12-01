using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSystem : MonoBehaviour
{
    public static PrefabSystem Instance { get; private set; }

    public List<Plant> plantPrefabs;
    public List<Zombie> zombiePrefabs;
    public List<Bullet> bulletPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public Plant GetPlantPrefab(PlantID plantID)
    {
        foreach (Plant plant in plantPrefabs)
        {
            if (plant.id == plantID) return plant;
        }
        return null;
    }
}
