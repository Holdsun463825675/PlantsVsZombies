using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelListKind
{
    None = 0,
    Adventure_Day = 1, Adventure_Night = 2, Adventure_Pool = 3, Adventure_Fog = 4, Adventure_Roof = 5,
    MiniGame_1 = 6,
}

public class LevelList : MonoBehaviour
{
    public LevelListKind kind;
    public bool needUnlocked;
    public List<GameObject> levelList;

    public void showLevels()
    {
        foreach (GameObject go in levelList)
        {
            if (!go) return;
            if (!JSONSaveSystem.Instance) // ≤‚ ‘
            {
                go.SetActive(true);
                continue;
            }
            bool unlocked = false;
            foreach (var level in JSONSaveSystem.Instance.userData.levelDatas)
            {
                if (level.levelID == int.Parse(go.name))
                {
                    unlocked = true;
                    if (level.completed) go.transform.Find("Completed").gameObject.SetActive(true);
                    else go.transform.Find("Completed").gameObject.SetActive(false);
                    break;
                }
            }
            go.SetActive(unlocked || !needUnlocked);
        }
    }

    public void SkipLevel()
    {
        if (!JSONSaveSystem.Instance) return;
        foreach (GameObject go in levelList)
        {
            LevelConfig levelConfig = LevelConfigManager.Instance.GetLevelConfig(int.Parse(go.name));
            JSONSaveSystem.Instance.CompleteLevel(levelConfig);
        }
    }
}
