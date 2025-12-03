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
        onLevelButtonClick(LevelListKind.Adventure_Day);
    }

    public void onMiniGameButtonClick()
    {
        onLevelButtonClick(LevelListKind.MiniGame_1);
    }
}
