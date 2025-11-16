using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAward : MonoBehaviour
{
    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void onClick()
    {
        if (anim.GetBool(AnimatorConfig.clicked)) return;
        anim.SetBool(AnimatorConfig.clicked, true);
        AudioManager.Instance.stopBgm();
        AudioManager.Instance.playClip(ResourceConfig.sound_win_winmusic);
    }

    public void onCompleteEnlarge()
    {
        GameManager.Instance.BacktoHome();
    }
}
