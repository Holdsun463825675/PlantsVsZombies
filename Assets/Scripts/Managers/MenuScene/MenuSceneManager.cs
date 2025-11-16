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

    public void onPlayGameButtonClick()
    {
        LevelConfigManager.Instance.GetLevelConfig(1);
        SceneManager.LoadScene("GameScene");
    }
}
