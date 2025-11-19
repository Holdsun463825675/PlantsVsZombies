using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.volume = SettingSystem.Instance.settingsData.music;
        playBgm(ResourceConfig.music_mainMenu);
    }

    public void playBgm(string path)
    {
        AudioClip ac = Resources.Load<AudioClip>(path);
        if (ac != null)
        {
            audioSource.clip = ac;
            audioSource.Play();
        }
    }

    public void stopBgm()
    {
        if (audioSource) audioSource.Stop();
    }

    public void playClip(string path)
    {
        AudioClip ac = Resources.Load<AudioClip>(path);
        if (ac != null)
        {
            AudioSource.PlayClipAtPoint(ac, transform.position, SettingSystem.Instance.settingsData.sound);
        }
    }
}
