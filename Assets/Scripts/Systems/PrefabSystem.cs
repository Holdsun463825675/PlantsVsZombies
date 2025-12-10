using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSystem : MonoBehaviour
{
    public static PrefabSystem Instance { get; private set; }

    public List<Plant> plantPrefabs;
    public List<Zombie> zombiePrefabs;
    public List<Bullet> bulletPrefabs;
    public List<Sun> sunPrefabs;
    public List<Dialog> dialogPrefabs;

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

    public Bullet GetBulletPrefab(BulletID bulletID)
    {
        foreach (Bullet bullet in bulletPrefabs)
        {
            if (bullet.id == bulletID) return bullet;
        }
        return null;
    }

    public Sun GetSunPrefab(SunID sunID)
    {
        foreach (Sun sun in sunPrefabs)
        {
            if (sun.id == sunID) return sun;
        }
        return null;
    }
}
