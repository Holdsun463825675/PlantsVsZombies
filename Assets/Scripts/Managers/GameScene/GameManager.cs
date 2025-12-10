using DG.Tweening;
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

    public LevelConfig currLevelConfig;
    private float gameSpeed;

    private void Awake()
    {
        Instance = this;
        DOTween.SetTweensCapacity(1000, 100);
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
        UIManager.Instance.PreviewingText.GetComponent<TextMeshProUGUI>().text = "关卡：" + currLevelConfig.levelName;
        UIManager.Instance.LevelText.GetComponent<TextMeshProUGUI>().text = "关卡：" + currLevelConfig.levelName;
        // 地图
        MapManager.Instance.setMap(currLevelConfig.mapID);
        CellManager.Instance.setMap(currLevelConfig.restrictedArea);
        CameraManager.Instance.getMap();
        CleanerManager.Instance.getMap();
        SunManager.Instance.getMap();
        ZombieManager.Instance.getMap();
        // 时间
        // 音乐
        // 是否掉落阳光
        SunManager.Instance.setIsDropSun(currLevelConfig.dropSun);
        // 初始阳光
        SunManager.Instance.setSun(currLevelConfig.startingSun);
        // 是否可用铲子
        CardManager.Instance.setConfigs(currLevelConfig.fixedCards, currLevelConfig.conveyorCards, currLevelConfig.shovel, currLevelConfig.generateCardTime);
        // 选卡类型：自选卡0、固定选卡1、传送带2
        // 僵尸
        ZombieManager.Instance.setConfig(
            currLevelConfig.zombieID,
            currLevelConfig.zombieWaves,
            currLevelConfig.specialZombies,
            currLevelConfig.spawnMaxTime,
            currLevelConfig.spawnTimer,
            currLevelConfig.healthPercentageThreshold);
        UIManager.Instance.initLevelProcess(currLevelConfig.zombieWaves);
        // 战利品
        setState(GameState.Previewing);
        // 固定游戏速度
        gameSpeed = currLevelConfig.gameSpeed;
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
                GameSpeedManager.Instance.SetGameSpeed(1.0f);
                UIManager.Instance.setState(GameState.Previewing);
                CardManager.Instance.setState(GameState.Previewing);
                AudioManager.Instance.playBgm(ResourceConfig.music_selectCard);
                CameraManager.Instance.setState(GameState.Previewing);
                CleanerManager.Instance.setState(GameState.Previewing);
                ZombieManager.Instance.setZombiePreviewing(true);
                break;
            case GameState.SelectingCard:
                UIManager.Instance.setState(GameState.SelectingCard);
                CameraManager.Instance.setState(GameState.SelectingCard);
                CleanerManager.Instance.setState(GameState.SelectingCard);
                CardManager.Instance.setState(GameState.SelectingCard);
                break;
            case GameState.Ready:
                UIManager.Instance.setState(GameState.Ready);
                CameraManager.Instance.setState(GameState.Ready);
                break;
            case GameState.Processing:
                GameSpeedManager.Instance.SetGameSpeed(gameSpeed != -1 ? gameSpeed : SettingSystem.Instance.settingsData.gameSpeed);
                AudioManager.Instance.playBgm(currLevelConfig.music);
                CleanerManager.Instance.setState(GameState.Processing);
                CardManager.Instance.setState(GameState.Processing);
                UIManager.Instance.setState(GameState.Processing);
                SunManager.Instance.setState(SunSpawnState.Enable);
                ZombieManager.Instance.setZombiePreviewing(false);
                ZombieManager.Instance.setState(ZombieSpawnState.Processing);
                break;
            case GameState.Paused:
                CleanerManager.Instance.setState(GameState.Paused);
                UIManager.Instance.setState(GameState.Paused);
                CardManager.Instance.Pause();
                PlantManager.Instance.Pause();
                ProductManager.Instance.Pause();
                ZombieManager.Instance.Pause();
                break;
            case GameState.Losing:
                GameSpeedManager.Instance.SetGameSpeed(1.0f);
                AudioManager.Instance.stopBgm();
                AudioManager.Instance.playClip(ResourceConfig.sound_lose_losemusic);
                CameraManager.Instance.setState(GameState.Losing);
                CleanerManager.Instance.setState(GameState.Losing);
                CardManager.Instance.setState(GameState.Losing);
                PlantManager.Instance.Pause();
                ProductManager.Instance.Pause();
                ZombieManager.Instance.Pause();
                break;
            case GameState.Winning:
                UIManager.Instance.setState(GameState.Winning);
                SunManager.Instance.setState(SunSpawnState.Disable);
                CardManager.Instance.setState(GameState.Winning);
                ZombieManager.Instance.setState(ZombieSpawnState.End);
                ZombieManager.Instance.killAllZombie(); // 胜利后击杀所有场上僵尸
                if (JSONSaveSystem.Instance) JSONSaveSystem.Instance.CompleteLevel(currLevelConfig); // 通关处理
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
    }

    public void Continue()
    {
        state = GameState.Processing;
        GameSpeedManager.Instance.SetGameSpeed(gameSpeed != -1 ? gameSpeed : SettingSystem.Instance.settingsData.gameSpeed);
        UIManager.Instance.setState(GameState.Processing);
        CleanerManager.Instance.setState(GameState.Processing);
        CardManager.Instance.Continue();
        PlantManager.Instance.Continue();
        ProductManager.Instance.Continue();
        ZombieManager.Instance.Continue();
    }

    public void BacktoHome()
    {
        GameSpeedManager.Instance.SetGameSpeed(1.0f);
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

    public void SkipLevel()
    {
        Continue();
        setState(GameState.Winning);
    }
}
