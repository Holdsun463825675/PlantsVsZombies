using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelConfig;

public enum GameState
{
    None,
    NotStarted,
    Previewing,
    SelectingCard,
    Ready,
    Processing,
    Paused,
    Losing,
    Winning
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;

    private LevelConfig currLevelConfig;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        setState(GameState.NotStarted);
        if (LevelConfigManager.Instance) currLevelConfig = LevelConfigManager.Instance.currLevelConfig;
        else currLevelConfig = LevelConfigs.levelConfigs[0]; // 默认加载测试关
        setLevelConfig();
    }

    private void setLevelConfig()
    {
        if (currLevelConfig == null) return;
        // 关卡ID
        // 关卡名称
        UIManager.Instance.PreviewingText.GetComponent<TextMeshProUGUI>().text = "Level: " + currLevelConfig.levelName;
        UIManager.Instance.LevelText.GetComponent<TextMeshProUGUI>().text = "Level: " + currLevelConfig.levelName;
        // 地图
        MapManager.Instance.setMap(currLevelConfig.mapID);
        CameraManager.Instance.getMap();
        SunManager.Instance.getMap();
        ZombieManager.Instance.getMap();
        // 时间
        // 是否掉落阳光
        SunManager.Instance.setIsDropSun(currLevelConfig.dropSun);
        // 初始阳光
        SunManager.Instance.setSun(currLevelConfig.startingSun);
        // 选卡类型：自选卡0、固定选卡1、传送带2
        // 僵尸
        ZombieManager.Instance.setConfig(
            currLevelConfig.zombieID,
            currLevelConfig.zombieWaves,
            currLevelConfig.spawnTime,
            currLevelConfig.spawnTimer,
            currLevelConfig.healthPercentageThreshold);
        UIManager.Instance.initLevelProcess(currLevelConfig.zombieWaves);
        // 战利品
        setState(GameState.Previewing);
    }

    public void setState(GameState state)
    {
        if (this.state == state) return;
        this.state = state;
        switch (state)
        {
            case GameState.NotStarted:
                break;
            case GameState.Previewing:
                UIManager.Instance.setState(GameState.Previewing);
                AudioManager.Instance.playBgm(ResourceConfig.music_selectCard);
                CameraManager.Instance.setState(GameState.Previewing);
                ZombieManager.Instance.setZombiePreviewing(true);
                break;
            case GameState.SelectingCard:
                UIManager.Instance.setState(GameState.SelectingCard);
                CameraManager.Instance.setState(GameState.SelectingCard);
                CardManager.Instance.setState(GameState.SelectingCard);
                break;
            case GameState.Ready:
                UIManager.Instance.setState(GameState.Ready);
                CameraManager.Instance.setState(GameState.Ready);
                CardManager.Instance.setState(GameState.Ready);
                break;
            case GameState.Processing:
                AudioManager.Instance.playBgm(ResourceConfig.music_day);
                CameraManager.Instance.setState(GameState.Processing);
                CardManager.Instance.setState(GameState.Processing);
                UIManager.Instance.setState(GameState.Processing);
                SunManager.Instance.setState(SunSpawnState.Enable);
                ZombieManager.Instance.setZombiePreviewing(false);
                ZombieManager.Instance.setState(ZombieSpawnState.Processing);
                break;
            case GameState.Paused:
                UIManager.Instance.setState(GameState.Paused);
                break;
            case GameState.Losing:
                CameraManager.Instance.setState(GameState.Losing);
                CardManager.Instance.setState(GameState.Losing);
                AudioManager.Instance.stopBgm();
                AudioManager.Instance.playClip(ResourceConfig.sound_lose_losemusic);
                PlantManager.Instance.Pause();
                ProductManager.Instance.Pause();
                SunManager.Instance.Pause();
                ZombieManager.Instance.Pause();
                break;
            case GameState.Winning:
                UIManager.Instance.setState(GameState.Winning);
                SunManager.Instance.setState(SunSpawnState.Disable);
                if (JSONSaveSystem.Instance) // 测试
                {
                    JSONSaveSystem.Instance.CompleteLevel(currLevelConfig.levelID);
                    JSONSaveSystem.Instance.UnlockLevel(currLevelConfig.nextLevelID);
                    JSONSaveSystem.Instance.unlockPlant(currLevelConfig.awardPlantID);
                }
                break;
            default:
                break;
        }
    }


    public void PauseAndContinue()
    {
        if (state == GameState.Processing) Pause();
        else if (state == GameState.Paused) Continue();
    }

    public void Pause()
    {
        setState(GameState.Paused);
        PlantManager.Instance.Pause();
        ProductManager.Instance.Pause();
        SunManager.Instance.Pause();
        ZombieManager.Instance.Pause();
    }

    public void Continue()
    {
        state = GameState.Processing;
        UIManager.Instance.setState(GameState.Processing);
        PlantManager.Instance.Continue();
        ProductManager.Instance.Continue();
        SunManager.Instance.Continue();
        ZombieManager.Instance.Continue();
    }

    public void BacktoHome()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        SceneManager.LoadScene("MenuScene");
        AudioManager.Instance.playBgm(ResourceConfig.music_mainMenu);
    }

    public void Restart()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
