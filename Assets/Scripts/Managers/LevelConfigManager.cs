using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigManager : MonoBehaviour
{
    public static LevelConfigManager Instance { get; private set; }

    private List<LevelConfig> levelConfigs;
    private Dictionary<int, LevelConfig> levelConfigDict;

    public LevelConfig currLevelConfig;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        InitializeLevelDictionary();
    }

    void InitializeLevelDictionary()
    {
        levelConfigs = LevelConfigs.levelConfigs;
        levelConfigDict = new Dictionary<int, LevelConfig>();
        foreach (var config in levelConfigs)
        {
            if (!levelConfigDict.ContainsKey(config.levelID))
            {
                levelConfigDict.Add(config.levelID, config);
            }
        }
    }

    public LevelConfig GetLevelConfig(int levelID)
    {
        if (levelConfigDict.ContainsKey(levelID))
        {
            currLevelConfig = levelConfigDict[levelID];
            return currLevelConfig;
        }
        else
        {
            Debug.LogError($"关卡配置不存在: Level_{levelID}");
            return null;
        }

    }

    public bool IsLevelUnlocked(int levelID)
    {
        // 检查关卡解锁条件
        int maxCompletedLevel = PlayerPrefs.GetInt("MaxCompletedLevel", 0);
        return levelID <= maxCompletedLevel + 1;
    }

    public List<LevelConfig> GetAvailableLevels()
    {
        List<LevelConfig> availableLevels = new List<LevelConfig>();
        foreach (var config in levelConfigs)
        {
            if (IsLevelUnlocked(config.levelID))
                availableLevels.Add(config);
        }
        return availableLevels;
    }
}
