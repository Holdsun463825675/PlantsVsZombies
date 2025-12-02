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

    public void playHitClip(BulletHitSound hitSound, int hitSoundPriority, ZombieUnderAttackSound underAttackSound, int underAttackSoundPriority)
    {
        string hitSoundPath = "", underAttackSoundPath = "";
        switch (hitSound)
        {
            case BulletHitSound.None:
                break;
            case BulletHitSound.Kernelpult:
                hitSoundPath = ResourceConfig.sound_bullethit_kernelpults[Random.Range(0, ResourceConfig.sound_bullethit_kernelpults.Length)];
                break;
            case BulletHitSound.Butter:
                hitSoundPath = ResourceConfig.sound_bullethit_butter;
                break;
            case BulletHitSound.Melon:
                hitSoundPath = ResourceConfig.sound_bullethit_melonimpacts[Random.Range(0, ResourceConfig.sound_bullethit_melonimpacts.Length)];
                break;
            case BulletHitSound.FirePea:
                hitSoundPath = ResourceConfig.sound_fire_ignites[Random.Range(0, ResourceConfig.sound_fire_ignites.Length)];
                break;
            case BulletHitSound.Bowling:
                hitSoundPath = ResourceConfig.sound_plant_bowlingimpact;
                break;
            default:
                break;
        }
        switch (underAttackSound)
        {
            case ZombieUnderAttackSound.Splat:
                underAttackSoundPath = ResourceConfig.sound_bullethit_splats[Random.Range(0, ResourceConfig.sound_bullethit_splats.Length)];
                break;
            case ZombieUnderAttackSound.Plastic:
                underAttackSoundPath = ResourceConfig.sound_bullethit_plastichits[Random.Range(0, ResourceConfig.sound_bullethit_plastichits.Length)];
                break;
            case ZombieUnderAttackSound.Shield:
                underAttackSoundPath = ResourceConfig.sound_bullethit_shieldhits[Random.Range(0, ResourceConfig.sound_bullethit_shieldhits.Length)];
                break;
            default:
                break;
        }
        if (hitSoundPriority == underAttackSoundPriority)
        {
            playClip(hitSoundPath); playClip(underAttackSoundPath);
        }
        else if (hitSoundPriority > underAttackSoundPriority) playClip(hitSoundPath);
        else playClip(underAttackSoundPath);
    }
}
