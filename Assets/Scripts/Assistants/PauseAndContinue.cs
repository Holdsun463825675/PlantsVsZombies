using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseAndContinue : MonoBehaviour
{
    public TextMeshProUGUI buttonText;


    public void onClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        GameManager.Instance.PauseAndContinue();
        if (GameManager.Instance.state == GameState.Paused) buttonText.text = "Continue";
        else buttonText.text = "Pause";
    }
}
