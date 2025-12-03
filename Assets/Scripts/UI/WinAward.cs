using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinAward : MonoBehaviour
{
    private Animator anim;
    private Button button;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    public void onClick()
    {
        button.enabled = false;
        if (anim.GetBool(AnimatorConfig.clicked)) return;
        anim.SetBool(AnimatorConfig.clicked, true);
        AudioManager.Instance.stopBgm();
        AudioManager.Instance.playClip(ResourceConfig.sound_win_winmusic);
    }

    public void onCompleteEnlarge()
    {
        AudioManager.Instance.playBgm(ResourceConfig.music_mainMenu);
        SceneManager.LoadScene("LevelScene");
    }
}
