using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public static StartSceneManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this; 
    }

    public void Start()
    {

    }

    public void onPlayButtonClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_buttonclick);
        SceneManager.LoadScene("MenuScene");
    }
}
