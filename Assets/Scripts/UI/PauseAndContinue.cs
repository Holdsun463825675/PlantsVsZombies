using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseAndContinue : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    public void onClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_gravebutton);
        GameManager.Instance.PauseAndContinue();
        if (GameManager.Instance.state == GameState.Paused) buttonText.text = "¼ÌÐø";
        else buttonText.text = "ÔÝÍ£";
    }
}
