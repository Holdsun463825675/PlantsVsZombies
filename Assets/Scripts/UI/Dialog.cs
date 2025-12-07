using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum DialogType
{
    None = 0, Confirmation = 1, Message = 2, Input = 3,
}

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button confirm;
    public TMP_InputField inputField;

    public void setText(string text)
    {
        this.text.text = text;
    }

    public void onButtonClick()
    {
        AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_tap2);
        if (DialogManager.Instance.currDialog == this) DialogManager.Instance.currDialog = null;
        Destroy(gameObject);
    }

    public void addConfirmAction(Action action)
    {
        confirm.onClick.AddListener(() => action?.Invoke());
        confirm.onClick.AddListener(() => { onButtonClick(); });
    }

    public string getInputText()
    {
        return inputField.text;
    }
}
