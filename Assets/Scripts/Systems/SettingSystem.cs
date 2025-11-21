using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONSaveSystem;

[System.Serializable]
public class SettingsData
{
    public float music;
    public float sound;
    public float difficulty;
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
            settingsData.difficulty = 1.0f;
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
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        AudioManager.Instance.audioSource.volume = volume;
        settingsData.music = volume;
        SaveSettingsData();
    }
    public void SetClipVolume(float volume)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        settingsData.sound = volume;
        SaveSettingsData();
    }
    public void SetDifficulty(float difficulty)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        settingsData.difficulty = difficulty;
        SaveSettingsData();
    }
    public void ToggleAutoCollected(bool autoCollected)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        settingsData.autoCollected = autoCollected;
        SaveSettingsData();
    }
    public void TogglePlantHealth(bool plantHealth)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        settingsData.plantHealth = plantHealth;
        SaveSettingsData();
    }
    public void ToggleZombieHealth(bool zombieHealth)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        settingsData.zombieHealth = zombieHealth;
        SaveSettingsData();
    }
}
