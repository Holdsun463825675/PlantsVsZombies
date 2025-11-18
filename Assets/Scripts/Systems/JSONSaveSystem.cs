using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONSaveSystem;

public class JSONSaveSystem : MonoBehaviour
{
    public static JSONSaveSystem Instance { get; private set; }
    private const string METADATA_KEY = "fghjfsdfghasdfghsdff";

    private Metadata metadata;
    public UserData userData;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMetadata();
            LoadCurrentUserData();
        }
        else Destroy(gameObject);
    }

    public class Metadata
    {
        public List<string> userIDs = new List<string>();
        public string currentUserID;
    }

    public class UserData
    {
        public string userID;
        public string name;
        public List<PlantID> unlockedPlants = new List<PlantID>();
        public List<LevelData> levelDatas = new List<LevelData>();
        public SettingsData settingsData = new SettingsData();
    }

    public class LevelData
    {
        public int levelID;
        public bool unlocked;
        public bool completed;
    }

    public class SettingsData
    {
        public float music;
        public float sound;
        public float difficulty;
        public bool autoCollected;
        public bool plantHealth;
        public bool zombieHealth;
    }

    private void LoadMetadata()
    {
        if (PlayerPrefs.HasKey(METADATA_KEY))
        {
            string json = PlayerPrefs.GetString(METADATA_KEY);
            metadata = JsonUtility.FromJson<Metadata>(json);
        }
        else metadata = new Metadata();
    }

    private void SaveMetadata()
    {
        string json = JsonUtility.ToJson(metadata, true);
        PlayerPrefs.SetString(METADATA_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadCurrentUserData()
    {
        if (string.IsNullOrEmpty(metadata.currentUserID))
        {
            CreateNewUser("Player");
            return;
        }

        LoadUserData(metadata.currentUserID);
    }

    public void LoadUserData(string userID)
    {
        string saveKey = $"UserData_{userID}";
        if (PlayerPrefs.HasKey(saveKey))
        {
            string jsonData = PlayerPrefs.GetString(saveKey);
            userData = JsonUtility.FromJson<UserData>(jsonData);
            metadata.currentUserID = userID;
            SaveMetadata();
            Debug.Log($"加载用户数据: {userData.name}");
        }
    }

    public void SaveGameData()
    {
        if (userData == null) return;

        string saveKey = $"UserData_{userData.userID}";
        string jsonData = JsonUtility.ToJson(userData, true);
        PlayerPrefs.SetString(saveKey, jsonData);
        PlayerPrefs.Save();

        Debug.Log($"游戏数据已保存: {userData.name}");
    }

    public void CreateNewUser(string userName)
    {
        string newUserID = Guid.NewGuid().ToString();
        userData = new UserData { userID = newUserID };
        InitializeDefaultData();
        metadata.userIDs.Add(newUserID);
        metadata.currentUserID = newUserID;

        SaveMetadata();
        SaveGameData();

        Debug.Log($"创建新用户: {userName} (ID: {newUserID})");
    }

    public void SwitchUser(string userID)
    {
        if (metadata.userIDs.Contains(userID))
        {
            LoadUserData(userID);
        }
    }

    public List<string> GetAllUserNames()
    {
        List<string> userNames = new List<string>();
        foreach (string userID in metadata.userIDs)
        {
            string saveKey = $"UserData_{userID}";
            if (PlayerPrefs.HasKey(saveKey))
            {
                string jsonData = PlayerPrefs.GetString(saveKey);
                UserData data = JsonUtility.FromJson<UserData>(jsonData);
                userNames.Add(data.name);
            }
        }
        return userNames;
    }

    private void InitializeDefaultData()
    {
        userData.name = "Player";
        userData.unlockedPlants.Add(PlantID.PeaShooter);
        userData.levelDatas.Add(new LevelData { levelID = 1, unlocked = true, completed = false });
        userData.settingsData = new SettingsData { music = 1.0f, sound = 1.0f, difficulty = 1.0f, autoCollected = false, plantHealth = false, zombieHealth = false };
        SaveGameData();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        userData.name = name;
        SaveGameData();
    }

    public void unlockPlant(PlantID ID)
    {
        if (userData.unlockedPlants.Contains(ID)) return;
        userData.unlockedPlants.Add(ID);
        SaveGameData();
    }

    public void CompleteLevel(int levelID)
    {
        foreach (LevelData level in userData.levelDatas)
        {
            if (level.levelID == levelID) level.completed = true;
        }
        SaveGameData();
    }

    public void UnlockLevel(int levelID)
    {
        foreach (LevelData level in userData.levelDatas)
        {
            if (level.levelID == levelID) return;
        }
        userData.levelDatas.Add(new LevelData { levelID = levelID, unlocked = true, completed = false });
        SaveGameData();
    }

    public void SaveSettings_Music(float music) { userData.settingsData.music = music; SaveGameData(); }
    public void SaveSettings_Sound(float sound) { userData.settingsData.sound = sound; SaveGameData(); }
    public void SaveSettings_Difficulty(float difficulty) { userData.settingsData.difficulty = difficulty; SaveGameData(); }
    public void SaveSettings_AutoCollected(bool autoCollected) { userData.settingsData.autoCollected = autoCollected; SaveGameData(); }
    public void SaveSettings_PlantHealth(bool plantHealth) { userData.settingsData.plantHealth = plantHealth; SaveGameData(); }
    public void SaveSettings_ZombieHealth(bool zombieHealth) { userData.settingsData.zombieHealth = zombieHealth; }

}
