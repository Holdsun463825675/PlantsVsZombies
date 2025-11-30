using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static JSONSaveSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject PreviewingText;
    public GameObject MenuButton;
    public GameObject Menu;
    public GameObject PauseAndContinue;
    public GameObject ReadytoPlay;
    public GameObject Approaching;
    public GameObject FinalWave;
    public GameObject LosingGame;
    public GameObject LosingGameButtons;
    public GameObject WinAward;
    public GameObject LevelText;
    public GameObject LevelProcess;
    public RectTransform flagBegin, flagEnd;

    public List<GameObject> Flags;

    private void Awake()
    {
        Instance = this;

        if (!PreviewingText) return;
        PreviewingText.SetActive(false);
        MenuButton.SetActive(false);
        Menu.SetActive(false);
        PauseAndContinue.SetActive(false);
        ReadytoPlay.SetActive(false);
        Approaching.SetActive(false);
        FinalWave.SetActive(false);
        LosingGame.SetActive(false);
        LosingGameButtons.SetActive(false);
        WinAward.SetActive(false);
        LevelText.SetActive(false);
        LevelProcess.SetActive(false);
    }

    private void Start()
    {
        if (!Menu) return;
        LoadSettingsToMenu();
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:

                break;
            case GameState.Previewing:
                PreviewingText.SetActive(true);
                break;
            case GameState.SelectingCard:
                PreviewingText.SetActive(false);
                MenuButton.SetActive(GameManager.Instance.currLevelConfig.cardType == TypeOfCard.Autonomy);
                break;
            case GameState.Ready:
                MenuButton.SetActive(false);
                Menu.SetActive(false);
                break;
            case GameState.Processing:
                ReadytoPlay.SetActive(false);
                MenuButton.SetActive(true);
                PauseAndContinue.SetActive(true);
                LevelText.SetActive(true);
                break;
            case GameState.Paused:
                MenuButton.SetActive(false);
                break;
            case GameState.Losing:
                MenuButton.SetActive(false);
                PauseAndContinue.SetActive(false);
                LosingGame.SetActive(true);
                LosingGame.GetComponent<Animator>().enabled = true;
                LosingGameButtons.SetActive(true);
                AudioManager.Instance.playClip(ResourceConfig.sound_lose_scream);
                break;
            case GameState.Winning:
                MenuButton.GetComponent<Button>().enabled = false;
                PauseAndContinue.GetComponent<Button>().enabled = false;
                setWinAward();
                WinAward.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void LoadSettingsToMenu()
    {
        // 获取所有UI组件
        Slider musicSlider = Menu.transform.Find("Music/MusicSlider").GetComponent<Slider>();
        Slider soundSlider = Menu.transform.Find("Sound/SoundSlider").GetComponent<Slider>();
        Slider gameSpeedSlider = Menu.transform.Find("GameSpeed/GameSpeedSlider").GetComponent<Slider>();
        Slider spawnMultiplierSlider = Menu.transform.Find("Difficulty/SpawnMultiplier/SpawnMultiplierSlider").GetComponent<Slider>();
        Slider hurtRateSlider = Menu.transform.Find("Difficulty/HurtRate/HurtRateSlider").GetComponent<Slider>();
        Toggle autoCollectedToggle = Menu.transform.Find("AutoCollected").GetComponent<Toggle>();
        Toggle plantHealthToggle = Menu.transform.Find("PlantHealth").GetComponent<Toggle>();
        Toggle zombieHealthToggle = Menu.transform.Find("ZombieHealth").GetComponent<Toggle>();

        AddAllListeners(musicSlider, soundSlider, gameSpeedSlider, spawnMultiplierSlider, hurtRateSlider);

        // 设置值
        musicSlider.value = SettingSystem.Instance.settingsData.music;
        soundSlider.value = SettingSystem.Instance.settingsData.sound;
        gameSpeedSlider.value = SettingConfig.gameSpeedMap.FirstOrDefault(x => Mathf.Approximately(x.Value, SettingSystem.Instance.settingsData.gameSpeed)).Key;
        spawnMultiplierSlider.value = SettingConfig.spawnMultiplierMap.FirstOrDefault(x => Mathf.Approximately(x.Value, SettingSystem.Instance.settingsData.spawnMultiplier)).Key;
        hurtRateSlider.value = SettingConfig.hurtRateMap.FirstOrDefault(x => Mathf.Approximately(x.Value, SettingSystem.Instance.settingsData.hurtRate)).Key;
        autoCollectedToggle.isOn = SettingSystem.Instance.settingsData.autoCollected;
        plantHealthToggle.isOn = SettingSystem.Instance.settingsData.plantHealth;
        zombieHealthToggle.isOn = SettingSystem.Instance.settingsData.zombieHealth;

        AddAllListeners(autoCollectedToggle, plantHealthToggle, zombieHealthToggle);
    }

    private void AddAllListeners(params Selectable[] uiElements)
    {
        foreach (Selectable element in uiElements)
        {
            if (element is Slider slider)
            {
                // 根据Slider名称添加不同的监听器
                switch (slider.name)
                {
                    case "MusicSlider":slider.onValueChanged.AddListener(OnMusicSliderChanged);break;
                    case "SoundSlider":slider.onValueChanged.AddListener(OnSoundSliderChanged);break;
                    case "GameSpeedSlider":slider.onValueChanged.AddListener(OnGameSpeedSliderChanged);break;
                    case "SpawnMultiplierSlider": slider.onValueChanged.AddListener(OnSpawnMultiplierSliderChanged);break;
                    case "HurtRateSlider": slider.onValueChanged.AddListener(OnHurtRateSliderChanged);break;
                }
            }
            else if (element is Toggle toggle)
            {
                // 根据Toggle名称添加不同的监听器
                switch (toggle.name)
                {
                    case "AutoCollected":toggle.onValueChanged.AddListener(OnAutoCollectedToggleChanged);break;
                    case "PlantHealth":toggle.onValueChanged.AddListener(OnPlantHealthToggleChanged);break;
                    case "ZombieHealth":toggle.onValueChanged.AddListener(OnZombieHealthToggleChanged);break;
                }
            }
        }
    }

    private void OnMusicSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetBgmVolume(sliderValue);
    }

    private void OnSoundSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetClipVolume(sliderValue);
    }

    private void OnGameSpeedSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetGameSpeed(sliderValue);
        float value = 1.0f;
        if (SettingConfig.gameSpeedMap.ContainsKey(sliderValue)) value = SettingConfig.gameSpeedMap[sliderValue];
        Menu.transform.Find("GameSpeed/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnSpawnMultiplierSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetSpawnMultiplier(sliderValue);
        float value = 1.0f;
        if (SettingConfig.spawnMultiplierMap.ContainsKey(sliderValue)) value = SettingConfig.spawnMultiplierMap[sliderValue];
        Menu.transform.Find("Difficulty/SpawnMultiplier/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnHurtRateSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetHurtRate(sliderValue);
        float value = 1.0f;
        if (SettingConfig.hurtRateMap.ContainsKey(sliderValue)) value = SettingConfig.hurtRateMap[sliderValue];
        Menu.transform.Find("Difficulty/HurtRate/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnAutoCollectedToggleChanged(bool isOn)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        SettingSystem.Instance.SetAutoCollected(isOn);
    }

    private void OnPlantHealthToggleChanged(bool isOn)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        SettingSystem.Instance.SetPlantHealth(isOn);
    }

    private void OnZombieHealthToggleChanged(bool isOn)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_ceramic);
        SettingSystem.Instance.SetZombieHealth(isOn);
    }


    // 复制RectTransform的所有属性（除了anchoredPosition.x）
    public void CopyRectTransformProperties(RectTransform source, RectTransform target, float newX)
    {
        if (source == null || target == null) return;

        // 2. 尺寸和偏移
        target.sizeDelta = source.sizeDelta;
        target.offsetMin = source.offsetMin;
        target.offsetMax = source.offsetMax;

        // 3. 旋转和缩放
        target.localRotation = source.localRotation;
        target.localScale = source.localScale;

        // 4. 其他重要属性
        target.localEulerAngles = source.localEulerAngles;

        // 1. 锚点和轴点设置
        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.anchoredPosition3D = new Vector3(newX, source.anchoredPosition3D.y, source.anchoredPosition3D.z);
    }

    public void initLevelProcess(List<ZombieWave> zombieWaves)
    {
        GameObject originalFlag = LevelProcess.transform.Find("Flag").gameObject;
        float currX = flagBegin.anchoredPosition.x;
        foreach (ZombieWave wave in zombieWaves)
        {
            currX -= (flagBegin.anchoredPosition.x - flagEnd.anchoredPosition.x) / zombieWaves.Count;
            // 大波插旗
            if (wave.largeWave)
            {
                GameObject newFlag = Instantiate(originalFlag);
                newFlag.transform.SetParent(originalFlag.transform.parent);
                newFlag.transform.SetSiblingIndex(originalFlag.transform.GetSiblingIndex() + 1);
                newFlag.GetComponent<Image>().SetNativeSize();
                CopyRectTransformProperties(originalFlag.GetComponent<RectTransform>(), newFlag.GetComponent<RectTransform>(), currX);
                newFlag.GetComponent<Animator>().enabled = false;
                newFlag.SetActive(true);
                Flags.Add(newFlag);
            }
        }
    }

    public void activateLevelProcess()
    {
        LevelProcess.SetActive(true);
    }

    public void setLevelProcess(float value)
    {
        LevelProcess.GetComponent<Slider>().value = value;
    }

    public void playReadytoPlay()
    {
        ReadytoPlay.SetActive(true);
        ReadytoPlay.GetComponent<Animator>().enabled = true;
        AudioManager.Instance.playClip(ResourceConfig.sound_textsound_readysetplant);
    }

    public void playHugeWave()
    {
        Approaching.GetComponent<Animator>().enabled = true;
        Approaching.SetActive(true);
        AudioManager.Instance.playClip(ResourceConfig.sound_textsound_hugewave);
    }

    public void playFinalWave()
    {
        FinalWave.GetComponent<Animator>().enabled = true;
        FinalWave.SetActive(true);
        AudioManager.Instance.playClip(ResourceConfig.sound_textsound_finalwave);
    }

    public void OpenMenu()
    {
        if (Menu.activeSelf) return;
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_pause);
        Menu.SetActive(true);
        if (GameManager.Instance.state == GameState.Processing) GameManager.Instance.Pause();
    }

    public void CloseMenu()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        Menu.SetActive(false);
        if (GameManager.Instance.state == GameState.Paused) GameManager.Instance.Continue();
    }

    private void setWinAward()
    {
        Image image = WinAward.GetComponent<Image>();
        if (GameManager.Instance.currLevelConfig.awardPlantID != PlantID.None) // 植物奖励
        {
            image.sprite = Resources.Load<Sprite>(ResourceConfig.image_plants + $"{GameManager.Instance.currLevelConfig.awardPlantID}" + "/Icon");
            return;
        }
        switch (GameManager.Instance.currLevelConfig.awardPropID) // 道具奖励
        {
            case PropID.None:
                break;
            case PropID.Shovel:
                image.sprite = Resources.Load<Sprite>(ResourceConfig.image_props + $"{GameManager.Instance.currLevelConfig.awardPropID}");
                break;
            default:
                break;
        }
    }
}
