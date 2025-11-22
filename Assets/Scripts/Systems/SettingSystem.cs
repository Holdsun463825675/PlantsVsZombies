using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONSaveSystem;

[System.Serializable]
public class SettingsData
{
    public float music;
    public float sound;
    public float gameSpeed;
    public float spawnMultiplier; // Ω© ¨≥ˆπ÷±∂¬ 
    public float hurtRate; // Ω© ¨ ‹…À±»¿˝
    public bool autoCollected;
    public bool plantHealth;
    public bool zombieHealth;
}

public class SettingSystem : MonoBehaviour
{
    public static SettingSystem Instance { get; private set; }

    public SettingsData settingsData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (!JSONSaveSystem.Instance) // ≤‚ ‘”√
        {
            settingsData = new SettingsData();
            settingsData.music = 1.0f;
            settingsData.sound = 1.0f;
            settingsData.gameSpeed = 1.0f;
            settingsData.spawnMultiplier = 1.0f;
            settingsData.hurtRate = 1.0f;
            settingsData.autoCollected = true;
            settingsData.plantHealth = true;
            settingsData.zombieHealth = true;
            return;
        }
        settingsData = JSONSaveSystem.Instance.userData.settingsData;
    }

    public void SaveSettingsData()
    {
        if (!JSONSaveSystem.Instance) return; // ≤‚ ‘”√
        JSONSaveSystem.Instance.SaveSettings(settingsData);
    }

    public void SetBgmVolume(float volume)
    {
        AudioManager.Instance.audioSource.volume = volume;
        settingsData.music = volume;
        SaveSettingsData();
    }
    public void SetClipVolume(float volume)
    {
        settingsData.sound = volume;
        SaveSettingsData();
    }
    public void SetGameSpeed(float level)
    {
        if (SettingConfig.gameSpeedMap.ContainsKey(level)) settingsData.gameSpeed = SettingConfig.gameSpeedMap[level];
        else settingsData.gameSpeed = 1.0f;
        SaveSettingsData();
    }
    public void SetSpawnMultiplier(float level)
    {
        if (SettingConfig.spawnMultiplierMap.ContainsKey(level)) settingsData.spawnMultiplier = SettingConfig.spawnMultiplierMap[level];
        else settingsData.spawnMultiplier = 1.0f;
        SaveSettingsData();
    }

    public void SetHurtRate(float level)
    {
        if (SettingConfig.hurtRateMap.ContainsKey(level)) settingsData.hurtRate = SettingConfig.hurtRateMap[level];
        else settingsData.hurtRate = 1.0f;
        SaveSettingsData();
    }

    public void SetAutoCollected(bool autoCollected)
    {
        settingsData.autoCollected = autoCollected;
        SaveSettingsData();
    }
    public void SetPlantHealth(bool plantHealth)
    {
        settingsData.plantHealth = plantHealth;
        SaveSettingsData();
    }
    public void SetZombieHealth(bool zombieHealth)
    {
        settingsData.zombieHealth = zombieHealth;
        SaveSettingsData();
    }
}
