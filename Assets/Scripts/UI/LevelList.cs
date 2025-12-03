using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelListKind
{
    None,
    Adventure_Day, Adventure_Night, Adventure_Pool, Adventure_Fog, Adventure_Roof,
    MiniGame_1,
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
}
