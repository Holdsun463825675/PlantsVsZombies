using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
                MenuButton.SetActive(true);
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
        Slider spawnMultiplierSlider = Menu.transform.Find("Difficulty/SpawnMultiplier/SpawnMultiplierSlider").GetComponent<Slider>();
        Slider hurtRateSlider = Menu.transform.Find("Difficulty/HurtRate/HurtRateSlider").GetComponent<Slider>();
        Toggle autoCollectedToggle = Menu.transform.Find("AutoCollected").GetComponent<Toggle>();
        Toggle plantHealthToggle = Menu.transform.Find("PlantHealth").GetComponent<Toggle>();
        Toggle zombieHealthToggle = Menu.transform.Find("ZombieHealth").GetComponent<Toggle>();

        AddAllListeners(musicSlider, soundSlider, spawnMultiplierSlider, hurtRateSlider);

        // 临时移除所有Toggle监听器
        RemoveAllListeners(autoCollectedToggle, plantHealthToggle, zombieHealthToggle);
        
        // 设置值
        musicSlider.value = SettingSystem.Instance.settingsData.music;
        soundSlider.value = SettingSystem.Instance.settingsData.sound;
        spawnMultiplierSlider.value = SettingConfig.spawnMultiplierMap.FirstOrDefault(x => Mathf.Approximately(x.Value, SettingSystem.Instance.settingsData.spawnMultiplier)).Key;
        hurtRateSlider.value = SettingConfig.hurtRateMap.FirstOrDefault(x => Mathf.Approximately(x.Value, SettingSystem.Instance.settingsData.hurtRate)).Key;
        autoCollectedToggle.isOn = SettingSystem.Instance.settingsData.autoCollected;
        plantHealthToggle.isOn = SettingSystem.Instance.settingsData.plantHealth;
        zombieHealthToggle.isOn = SettingSystem.Instance.settingsData.zombieHealth;

        // 重新添加Toggle监听器
        AddAllListeners(autoCollectedToggle, plantHealthToggle, zombieHealthToggle);
    }

    // 临时移除监听器
    private void RemoveAllListeners(params Selectable[] uiElements)
    {
        foreach (Selectable element in uiElements)
        {
            if (element is Slider slider)
            {
                slider.onValueChanged = new Slider.SliderEvent();
            }
            else if (element is Toggle toggle)
            {
                toggle.onValueChanged = new Toggle.ToggleEvent();
            }
        }
    }

    // 重新添加监听器
    private void AddAllListeners(params Selectable[] uiElements)
    {
        foreach (Selectable element in uiElements)
        {
            if (element is Slider slider)
            {
                // 根据Slider名称添加不同的监听器
                switch (slider.name)
                {
                    case "MusicSlider":slider.onValueChanged.AddListener(SettingSystem.Instance.SetBgmVolume);break;
                    case "SoundSlider":slider.onValueChanged.AddListener(SettingSystem.Instance.SetClipVolume);break;
                    case "SpawnMultiplierSlider": slider.onValueChanged.AddListener(OnSpawnMultiplierSliderChanged);break;
                    case "HurtRateSlider": slider.onValueChanged.AddListener(OnHurtRateSliderChanged);break;
                }
            }
            else if (element is Toggle toggle)
            {
                // 根据Toggle名称添加不同的监听器
                switch (toggle.name)
                {
                    case "AutoCollected":toggle.onValueChanged.AddListener(SettingSystem.Instance.ToggleAutoCollected);break;
                    case "PlantHealth":toggle.onValueChanged.AddListener(SettingSystem.Instance.TogglePlantHealth);break;
                    case "ZombieHealth":toggle.onValueChanged.AddListener(SettingSystem.Instance.ToggleZombieHealth);break;
                }
            }
        }
    }

    private void OnSpawnMultiplierSliderChanged(float sliderValue)
    {
        SettingSystem.Instance.SetSpawnMultiplier(sliderValue);
        float value = 1.0f;
        if (SettingConfig.spawnMultiplierMap.ContainsKey(sliderValue)) value = SettingConfig.spawnMultiplierMap[sliderValue];
        Menu.transform.Find("Difficulty/SpawnMultiplier/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnHurtRateSliderChanged(float sliderValue)
    {
        SettingSystem.Instance.SetHurtRate(sliderValue);
        float value = 1.0f;
        if (SettingConfig.hurtRateMap.ContainsKey(sliderValue)) value = SettingConfig.hurtRateMap[sliderValue];
        Menu.transform.Find("Difficulty/HurtRate/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
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


}
