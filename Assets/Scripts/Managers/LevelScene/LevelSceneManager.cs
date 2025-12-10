using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelSceneManager : MonoBehaviour
{
    public static LevelSceneManager Instance { get; private set; }

    public List<LevelList> levelLists_Adventure;
    public List<LevelList> levelLists_MiniGame;

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
        foreach (LevelList levelList in levelLists_Adventure)
        {
            if (levelList.kind == kind)
            {
                levelList.gameObject.SetActive(true);
                levelList.showLevels();
            }
            else levelList.gameObject.SetActive(false);
        }
        foreach (LevelList levelList in levelLists_MiniGame)
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

    public void onSkipLevelClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        DialogManager.Instance.createDialog(DialogType.Confirmation, DialogConfig.level_skipLevel, SkipLevel);
    }

    public void SkipLevel()
    {
        LevelListKind kind = LevelConfigManager.Instance ? LevelConfigManager.Instance.currLevelListKind : LevelListKind.Adventure_Day;
        foreach (LevelList levelList in levelLists_Adventure) if (levelList.kind == kind) levelList.SkipLevel();
        foreach (LevelList levelList in levelLists_MiniGame) if (levelList.kind == kind) levelList.SkipLevel();
        showLevelList(kind);
    }

    public void onPageButtonsClick(bool nextPage)
    {
        LevelListKind kind = LevelConfigManager.Instance ? LevelConfigManager.Instance.currLevelListKind : LevelListKind.Adventure_Day;
        List<LevelList> lists = null; int currListIdx = 0;
        for (int i = 0; i < levelLists_Adventure.Count; i++)
        {
            if (levelLists_Adventure[i].kind == kind)
            {
                lists = levelLists_Adventure;
                currListIdx = i;
            }
        }
        for (int i = 0; i < levelLists_MiniGame.Count; i++)
        {
            if (levelLists_MiniGame[i].kind == kind)
            {
                lists = levelLists_MiniGame;
                currListIdx = i;
            }
        }
        if (lists == null) return;
        if (nextPage) currListIdx = (currListIdx + 1) % lists.Count;
        else currListIdx = (currListIdx + lists.Count - 1) % lists.Count;
        if (LevelConfigManager.Instance) LevelConfigManager.Instance.currLevelListKind = lists[currListIdx].kind;
        if (JSONSaveSystem.Instance) // 解锁了才显示
        {
            if (JSONSaveSystem.Instance.userData.unlockedLevelListKinds.Contains(lists[currListIdx].kind)) showLevelList(lists[currListIdx].kind);
        }
        else showLevelList(lists[currListIdx].kind);
    }
}
