using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneUIManager : MonoBehaviour
{
    public static MenuSceneUIManager Instance { get; private set; }

    public GameObject SettingsMenu;
    public GameObject Users;
    public GameObject UsersMenu;
    private TMP_Dropdown UsersDropdown;

    private string currUserID; // 当前场景活跃的userID
    private string selectedUserID;

    public void Awake()
    {
        Instance = this;

        if (!SettingsMenu) return;
        SettingsMenu.SetActive(false);
        Users.transform.Find("UserLabel/Text").GetComponent<TextMeshProUGUI>().text = JSONSaveSystem.Instance.userData.name;
        Users.SetActive(true);
        UsersMenu.SetActive(false);
        UsersDropdown = UsersMenu.transform.Find("UsersDropdown").GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        if (!SettingsMenu) return;
        currUserID = JSONSaveSystem.Instance.metadata.currentUserID;
        selectedUserID = JSONSaveSystem.Instance.metadata.currentUserID;
        LoadSettingsToMenu();
        LoadUsersToMenu();
    }

    private void LoadSettingsToMenu()
    {
        // 获取所有UI组件
        Slider musicSlider = SettingsMenu.transform.Find("Music/MusicSlider").GetComponent<Slider>();
        Slider soundSlider = SettingsMenu.transform.Find("Sound/SoundSlider").GetComponent<Slider>();
        Slider gameSpeedSlider = SettingsMenu.transform.Find("GameSpeed/GameSpeedSlider").GetComponent<Slider>();
        Slider spawnMultiplierSlider = SettingsMenu.transform.Find("Difficulty/SpawnMultiplier/SpawnMultiplierSlider").GetComponent<Slider>();
        Slider hurtRateSlider = SettingsMenu.transform.Find("Difficulty/HurtRate/HurtRateSlider").GetComponent<Slider>();
        Toggle autoCollectedToggle = SettingsMenu.transform.Find("AutoCollected").GetComponent<Toggle>();
        Toggle plantHealthToggle = SettingsMenu.transform.Find("PlantHealth").GetComponent<Toggle>();
        Toggle zombieHealthToggle = SettingsMenu.transform.Find("ZombieHealth").GetComponent<Toggle>();

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
                    case "MusicSlider": slider.onValueChanged.AddListener(OnMusicSliderChanged); break;
                    case "SoundSlider": slider.onValueChanged.AddListener(OnSoundSliderChanged); break;
                    case "GameSpeedSlider": slider.onValueChanged.AddListener(OnGameSpeedSliderChanged); break;
                    case "SpawnMultiplierSlider": slider.onValueChanged.AddListener(OnSpawnMultiplierSliderChanged); break;
                    case "HurtRateSlider": slider.onValueChanged.AddListener(OnHurtRateSliderChanged); break;
                }
            }
            else if (element is Toggle toggle)
            {
                // 根据Toggle名称添加不同的监听器
                switch (toggle.name)
                {
                    case "AutoCollected": toggle.onValueChanged.AddListener(OnAutoCollectedToggleChanged); break;
                    case "PlantHealth": toggle.onValueChanged.AddListener(OnPlantHealthToggleChanged); break;
                    case "ZombieHealth": toggle.onValueChanged.AddListener(OnZombieHealthToggleChanged); break;
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
        SettingsMenu.transform.Find("GameSpeed/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnSpawnMultiplierSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetSpawnMultiplier(sliderValue);
        float value = 1.0f;
        if (SettingConfig.spawnMultiplierMap.ContainsKey(sliderValue)) value = SettingConfig.spawnMultiplierMap[sliderValue];
        SettingsMenu.transform.Find("Difficulty/SpawnMultiplier/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
    }

    private void OnHurtRateSliderChanged(float sliderValue)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SettingSystem.Instance.SetHurtRate(sliderValue);
        float value = 1.0f;
        if (SettingConfig.hurtRateMap.ContainsKey(sliderValue)) value = SettingConfig.hurtRateMap[sliderValue];
        SettingsMenu.transform.Find("Difficulty/HurtRate/Instruction").GetComponent<TextMeshProUGUI>().text = "×" + value.ToString("F2");
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

    private void LoadUsersToMenu()
    {
        if (UsersDropdown == null || JSONSaveSystem.Instance.metadata == null) return;

        UsersDropdown.ClearOptions();
        if (JSONSaveSystem.Instance.metadata.userIDs == null || JSONSaveSystem.Instance.metadata.userIDs.Count == 0)
        {
            UsersDropdown.options.Add(new TMP_Dropdown.OptionData("No Players"));
            UsersDropdown.interactable = false;
            UsersDropdown.RefreshShownValue();
            return;
        }

        // 生成下拉菜单选项
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();

        foreach (string userID in JSONSaveSystem.Instance.metadata.userIDs)
        {
            // 从保存的数据中获取用户名，如果没有则使用默认名称
            string userName = JSONSaveSystem.Instance.metadata.userNames[userID];
            string displayText = $"{userName}";
            dropdownOptions.Add(new TMP_Dropdown.OptionData(displayText));
        }

        // 设置下拉菜单选项
        UsersDropdown.AddOptions(dropdownOptions);
        UsersDropdown.interactable = true;

        // 设置当前选中的用户
        SetSelectedUser(selectedUserID);

        // 添加值改变监听
        UsersDropdown.onValueChanged.RemoveAllListeners();
        UsersDropdown.onValueChanged.AddListener(OnUserSelected);

        Debug.Log($"成功加载 {JSONSaveSystem.Instance.metadata.userIDs.Count} 个用户到下拉菜单");
    }

    private void SetSelectedUser(string currentUserID)
    {
        if (string.IsNullOrEmpty(currentUserID))
        {
            UsersDropdown.value = 0;
            UsersDropdown.RefreshShownValue();
            return;
        }

        int userIndex = JSONSaveSystem.Instance.metadata.userIDs.IndexOf(currentUserID);
        if (userIndex >= 0 && userIndex < UsersDropdown.options.Count)
        {
            UsersDropdown.value = userIndex;
            UsersDropdown.RefreshShownValue();
            Debug.Log($"设置当前用户: {UsersDropdown.options[userIndex].text}");
        }
        else
        {
            UsersDropdown.value = 0;
            UsersDropdown.RefreshShownValue();
            Debug.LogWarning($"未找到当前用户ID: {currentUserID}，使用第一个用户");
        }
    }

    private void OnUserSelected(int dropdownIndex)
    {
        if (dropdownIndex < 0 || dropdownIndex >= JSONSaveSystem.Instance.metadata.userIDs.Count)
        {
            Debug.LogError("选择的用户索引无效");
            return;
        }

        selectedUserID = JSONSaveSystem.Instance.metadata.userIDs[dropdownIndex];
        string selectedUserName = JSONSaveSystem.Instance.metadata.userNames.ContainsKey(selectedUserID)
            ? JSONSaveSystem.Instance.metadata.userNames[selectedUserID]
            : "Unknown player";

        Debug.Log($"用户选择: {selectedUserName} (ID: {selectedUserID})");
    }

    public void onNewClick()
    {
        JSONSaveSystem.Instance.CreateNewUser(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onDeleteClick()
    {
        if (selectedUserID == currUserID) return; // 无法删除当前活跃的用户
        JSONSaveSystem.Instance.DeleteUser(selectedUserID);
        selectedUserID = JSONSaveSystem.Instance.metadata.currentUserID;
        LoadUsersToMenu();
    }

    public void onOKClick()
    {
        if (JSONSaveSystem.Instance.metadata.currentUserID == selectedUserID) setUsersMenu(false);
        JSONSaveSystem.Instance.metadata.currentUserID = selectedUserID;
        JSONSaveSystem.Instance.SaveMetadata();
        JSONSaveSystem.Instance.LoadUserData(selectedUserID);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onCancelClick()
    {
        selectedUserID = JSONSaveSystem.Instance.metadata.currentUserID;
        setUsersMenu(false);
        if (currUserID != JSONSaveSystem.Instance.metadata.currentUserID) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void setSettingsMenu(bool isOn)
    {
        if (isOn && SettingsMenu.activeSelf) return;
        if (isOn) AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_pause);
        else AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        SettingsMenu.SetActive(isOn);
    }

    public void setUsersMenu(bool isOn)
    {
        LoadUsersToMenu();
        UsersMenu.SetActive(isOn);
    }

}
