using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardState
{
    SelectingCard_NotSelected,
    SelectingCard_Selected,
    GameReady,
    Ready,
    WaitingSun,
    CoolingDown,
    Selected
}

public class Card : MonoBehaviour
{
    private CardState state = CardState.SelectingCard_NotSelected;

    public PlantID plantID;
    public float cdTime = 7.5f;
    public float cdTimer = 0.0f;
    public int cost = 100;

    public GameObject Card_Original;
    public Image Card_Mask_Unavailable;
    public Image Card_Mask_CoolingDown;


    void FixedUpdate()
    {
        Card_Mask_CoolingDown.fillAmount = (cdTime - cdTimer) / cdTime;
        if (GameManager.Instance.state == GameState.Paused && state == CardState.CoolingDown) return;

        switch (state)
        {
            case CardState.SelectingCard_NotSelected:
                break;
            case CardState.SelectingCard_Selected:
                break;
            case CardState.GameReady:
                break;
            case CardState.Ready:
                ReadyUpdate();
                break;
            case CardState.WaitingSun:
                WaitingSunUpdate();
                break;
            case CardState.CoolingDown:
                CoolingDownUpdate();
                break;
            case CardState.Selected:
                SelectedUpdate();
                break;
            default:
                break;
        }
    }

    public void setState(CardState state)
    {
        switch (state)
        {
            case CardState.SelectingCard_NotSelected:
                Card_Mask_Unavailable.gameObject.SetActive(false);
                Card_Mask_CoolingDown.gameObject.SetActive(false);
                break;
            case CardState.SelectingCard_Selected:
                Card_Mask_Unavailable.gameObject.SetActive(false);
                Card_Mask_CoolingDown.gameObject.SetActive(false);
                break;
            case CardState.GameReady:
                Card_Mask_CoolingDown.fillAmount = 1.0f;
                Card_Mask_Unavailable.gameObject.SetActive(true);
                Card_Mask_CoolingDown.gameObject.SetActive(true);
                break;
            case CardState.Ready:
                Card_Mask_Unavailable.gameObject.SetActive(false);
                Card_Mask_CoolingDown.gameObject.SetActive(false);
                break;
            case CardState.WaitingSun:
                Card_Mask_Unavailable.gameObject.SetActive(true);
                Card_Mask_CoolingDown.gameObject.SetActive(false);
                break;
            case CardState.CoolingDown:
                Card_Mask_Unavailable.gameObject.SetActive(true);
                Card_Mask_CoolingDown.gameObject.SetActive(true);
                break;
            case CardState.Selected:
                Card_Mask_Unavailable.gameObject.SetActive(true);
                Card_Mask_CoolingDown.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        this.state = state;
    }

    private void ReadyUpdate()
    {
        if (cost > SunManager.Instance.Sun) setState(CardState.WaitingSun);
    }

    private void WaitingSunUpdate()
    {
        if (cost <= SunManager.Instance.Sun) setState(CardState.Ready);
    }

    private void CoolingDownUpdate()
    {
        cdTimer += Time.fixedDeltaTime;

        if (cdTimer >= cdTime)
        {
            setState(CardState.WaitingSun);
            cdTimer = 0.0f;
        }
    }

    private void SelectedUpdate()
    {

    }

    public void OnClick()
    {
        switch (state)
        {
            case CardState.SelectingCard_NotSelected:
                bool f0 = CardManager.Instance.addCard(this);
                if (f0)
                {
                    setState(CardState.SelectingCard_Selected);
                    AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_tap);
                } 
                break;
            case CardState.SelectingCard_Selected:
                bool f1 = CardManager.Instance.removeCard(this);
                if (f1)
                {
                    setState(CardState.SelectingCard_NotSelected);
                    AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_tap2);
                } 
                break;
            case CardState.GameReady:
                break;
            case CardState.Ready:
                HandManager.Instance.SuspendPlant(this, plantID);
                break;
            case CardState.WaitingSun:
                AudioManager.Instance.playClip(ResourceConfig.sound_clickfail_buzzer);
                break;
            case CardState.CoolingDown:
                AudioManager.Instance.playClip(ResourceConfig.sound_clickfail_buzzer);
                break;
            case CardState.Selected:
                HandManager.Instance.CancelPlant();
                break;
            default:
                break;
        }
    }
}