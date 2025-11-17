using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureSceneManager : MonoBehaviour
{
    public static AdventureSceneManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

    }

    public void onBacktoHomeButtonClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SceneManager.LoadScene("MenuScene");
    }

    public void onLevelButtonClick(int LevelID=0)
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        LevelConfigManager.Instance.GetLevelConfig(LevelID);
        SceneManager.LoadScene("GameScene");
    }
}
