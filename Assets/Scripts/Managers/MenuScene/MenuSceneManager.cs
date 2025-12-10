using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public static MenuSceneManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

    }

    public void onLevelButtonClick(LevelListKind kind=LevelListKind.Adventure_Day)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        LevelConfigManager.Instance.setCurrLevelListKind(kind);
        SceneManager.LoadScene("LevelScene");
    }

    public void onAdventureButtonClick()
    {
        LevelListKind kind = LevelListKind.Adventure_Day;
        // 显示已经进行到的冒险模式章节
        if (JSONSaveSystem.Instance)
        {
            if (!JSONSaveSystem.Instance.userData.unlockedLevelListKinds.Contains(LevelListKind.Adventure_Day)) // 自动解锁白天
            {
                JSONSaveSystem.Instance.unlockLevelListKind(new List<LevelListKind> { LevelListKind.Adventure_Day });
            }
            kind = findLastAdventureKind(JSONSaveSystem.Instance.userData.unlockedLevelListKinds);
        }
        onLevelButtonClick(kind);
    }

    public void onMiniGameButtonClick()
    {
        onLevelButtonClick(LevelListKind.MiniGame_1);
    }

    private LevelListKind findLastAdventureKind(List<LevelListKind> kinds)
    {
        LevelListKind currKind = LevelListKind.Adventure_Day;
        foreach (LevelListKind kind in kinds)
        {
            if (kind >= LevelListKind.Adventure_Day && kind <= LevelListKind.Adventure_Roof && kind > currKind) currKind = kind;
        }
        return currKind;
    }
}
