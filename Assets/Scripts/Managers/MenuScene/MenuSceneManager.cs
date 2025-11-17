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

    public void onAdventureButtonClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_bleep);
        SceneManager.LoadScene("AdventureScene");
    }
}
