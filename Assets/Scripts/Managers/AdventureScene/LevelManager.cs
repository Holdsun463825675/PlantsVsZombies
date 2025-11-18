using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public List<GameObject> LevelList;

    private void Awake()
    {
        Instance = this;
        showLevels();
    }

    private void Start()
    {
        
    }

    private void showLevels()
    {
        foreach (GameObject go in LevelList)
        {
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
            go.SetActive(unlocked);
        }
    }
}
