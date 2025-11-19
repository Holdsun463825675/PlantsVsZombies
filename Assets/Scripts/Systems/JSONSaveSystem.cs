using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONSaveSystem;

public class JSONSaveSystem : MonoBehaviour
{
    public static JSONSaveSystem Instance { get; private set; }
    private const string METADATA_KEY = "MetaData_1";

    public Metadata metadata;
    public UserData userData;
    

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
        LoadMetadata();
        LoadCurrentUserData();
    }

    [System.Serializable]
    public class Metadata
    {
        public List<string> userIDs = new List<string>();
        public SerializableDictionary<string, string> userNames = new SerializableDictionary<string, string>();
        public string currentUserID;
    }

    [System.Serializable]
    public class UserData
    {
        public string userID;
        public string name;
        public List<PlantID> unlockedPlants = new List<PlantID>();
        public List<LevelData> levelDatas = new List<LevelData>();
        public SettingsData settingsData = new SettingsData();
    }

    [System.Serializable]
    public class LevelData
    {
        public int levelID;
        public bool unlocked;
        public bool completed;
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

    public void SaveMetadata()
    {
        string json = JsonUtility.ToJson(metadata, true);
        PlayerPrefs.SetString(METADATA_KEY, json);
        PlayerPrefs.Save();
    }

    public void LoadCurrentUserData()
    {
        if (string.IsNullOrEmpty(metadata.currentUserID))
        {
            CreateNewUser(true);
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
            SettingSystem.Instance.settingsData = userData.settingsData;
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

    public void CreateNewUser(bool selected=false)
    {
        UserData prev_userData = userData;
        string newUserID = Guid.NewGuid().ToString();
        UserData newUserData = new UserData { 
            userID = newUserID,
            name = $"Player_{newUserID.Substring(0, 8)}",
            unlockedPlants = { PlantID.PeaShooter },
            levelDatas = { new LevelData { levelID = 1, unlocked = true, completed = false } },
            settingsData = new SettingsData { music = 1.0f, sound = 1.0f, difficulty = 1.0f, autoCollected = false, plantHealth = false, zombieHealth = false },
        };
        metadata.userIDs.Add(newUserID);
        metadata.userNames[newUserID] = newUserData.name;
        userData = newUserData;

        SaveMetadata();
        SaveGameData();

        if (!selected) userData = prev_userData;

        Debug.Log($"创建新用户: {newUserData.name} (ID: {newUserID})");
    }

    public string GetUserNameByID(string userID)
    {
        string saveKey = $"UserData_{userID}";

        if (PlayerPrefs.HasKey(saveKey))
        {
            try
            {
                string jsonData = PlayerPrefs.GetString(saveKey);
                UserData userData = JsonUtility.FromJson<UserData>(jsonData);
                return userData?.name ?? $"Player_{userID.Substring(0, 8)}";
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"读取用户 {userID} 数据失败: {e.Message}");
            }
        }

        // 默认用户名
        return $"Player_{userID.Substring(0, 8)}";
    }

    public void DeleteUser(string userID)
    {
        // 检查要删除的用户是否存在
        if (!metadata.userIDs.Contains(userID)) return;

        // 如果要删除的是当前用户，需要处理当前用户的切换
        if (metadata.currentUserID == userID)
        {
            metadata.userIDs.Remove(userID);
            metadata.userNames.Remove(userID);

            // 如果有其他用户，切换到第一个用户
            if (metadata.userIDs.Count > 0)
            {
                string newCurrentUserID = metadata.userIDs[0];
                metadata.currentUserID = newCurrentUserID;
                LoadUserData(newCurrentUserID);
                Debug.Log($"已切换到用户: {userData.name}");
            }
            else
            {
                // 如果没有其他用户，清空当前用户数据并创建新用户
                metadata.currentUserID = null;
                userData = null;
                CreateNewUser(true);
                Debug.Log("所有用户已删除，创建了新用户");
            }
        }
        else
        {
            // 如果要删除的不是当前用户，直接移除
            metadata.userIDs.Remove(userID);
            metadata.userNames.Remove(userID);
        }

        // 删除该用户的保存数据
        string saveKey = $"UserData_{userID}";
        if (PlayerPrefs.HasKey(saveKey))
        {
            PlayerPrefs.DeleteKey(saveKey);
            Debug.Log($"已删除用户数据: {saveKey}");
        }

        // 保存更新后的元数据
        SaveMetadata();

        Debug.Log($"用户删除完成: {userID}");
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

    public void SaveSettings(SettingsData data)
    {
        userData.settingsData = data;
        SaveGameData();
    }
}
