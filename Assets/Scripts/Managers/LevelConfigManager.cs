using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelConfigManager : MonoBehaviour
{
    public static LevelConfigManager Instance { get; private set; }

    private List<LevelConfig> levelConfigs;
    private Dictionary<int, LevelConfig> levelConfigDict;

    public LevelListKind currLevelListKind;
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

    public void setCurrLevelListKind(LevelListKind kind)
    {
        currLevelListKind = kind;
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
            Debug.LogError($"πÿø®≈‰÷√≤ª¥Ê‘⁄: Level_{levelID}");
            return null;
        }

    }
}
