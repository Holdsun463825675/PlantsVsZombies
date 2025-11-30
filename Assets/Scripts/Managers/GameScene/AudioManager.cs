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
        else Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playBgm(ResourceConfig.music_mainMenu);
    }

    private void Update()
    {
        audioSource.volume = SettingSystem.Instance.settingsData.music;
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

    public void playHitClip(Bullet bullet, Zombie zombie)
    {
        string hitSound = "", underAttackSound = "";
        switch (bullet.hitSound)
        {
            case BulletHitSound.None:
                break;
            case BulletHitSound.Kernelpult:
                hitSound = ResourceConfig.sound_bullethit_kernelpults[Random.Range(0, ResourceConfig.sound_bullethit_kernelpults.Length)];
                break;
            case BulletHitSound.Butter:
                hitSound = ResourceConfig.sound_bullethit_butter;
                break;
            case BulletHitSound.Melon:
                hitSound = ResourceConfig.sound_bullethit_melonimpacts[Random.Range(0, ResourceConfig.sound_bullethit_melonimpacts.Length)];
                break;
        }
        switch (zombie.underAttackSound)
        {
            case ZombieUnderAttackSound.Splat:
                underAttackSound = ResourceConfig.sound_bullethit_splats[Random.Range(0, ResourceConfig.sound_bullethit_splats.Length)];
                break;
            case ZombieUnderAttackSound.Plastic:
                underAttackSound = ResourceConfig.sound_bullethit_plastichits[Random.Range(0, ResourceConfig.sound_bullethit_plastichits.Length)];
                break;
            case ZombieUnderAttackSound.Shield:
                underAttackSound = ResourceConfig.sound_bullethit_shieldhits[Random.Range(0, ResourceConfig.sound_bullethit_shieldhits.Length)];
                break;
        }
        if (bullet.hitSoundPriority == zombie.underAttackSoundPriority)
        {
            playClip(hitSound); playClip(underAttackSound);
        }
        else if (bullet.hitSoundPriority > zombie.underAttackSoundPriority) playClip(hitSound);
        else playClip(underAttackSound);
    }
}
