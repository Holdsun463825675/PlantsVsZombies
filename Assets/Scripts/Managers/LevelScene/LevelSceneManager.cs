using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSceneManager : MonoBehaviour
{
    public static LevelSceneManager Instance { get; private set; }

    public List<LevelList> levelLists;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        AudioManager.Instance.playBgm(ResourceConfig.music_selectCard);
        // 默认加载冒险模式-白天
        if (!LevelConfigManager.Instance) showLevelList(LevelListKind.Adventure_Day);
        else showLevelList(LevelConfigManager.Instance.currLevelListKind);
    }

    private void showLevelList(LevelListKind kind)
    {
        foreach (LevelList levelList in levelLists)
        {
            if (levelList.kind == kind)
            {
                levelList.gameObject.SetActive(true);
                levelList.showLevels();
            }
            else levelList.gameObject.SetActive(false);
        }
    }

    public void onBacktoHomeButtonClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        AudioManager.Instance.playBgm(ResourceConfig.music_mainMenu);
        SceneManager.LoadScene("MenuScene");
    }

    public void onLevelButtonClick(int LevelID=0)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        LevelConfigManager.Instance.GetLevelConfig(LevelID);
        SceneManager.LoadScene("GameScene");
    }
}
