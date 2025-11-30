using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static JSONSaveSystem;

public class KeyboardInputSystem : MonoBehaviour
{
    public static KeyboardInputSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) OnEscapePressed();
        if (Input.GetKeyDown(KeyCode.Space)) OnSpacePressed();
        if (Input.GetKeyDown(KeyCode.Alpha0)) OnNumberKeyPressed(0);
        if (Input.GetKeyDown(KeyCode.Alpha1)) OnNumberKeyPressed(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) OnNumberKeyPressed(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) OnNumberKeyPressed(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) OnNumberKeyPressed(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) OnNumberKeyPressed(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) OnNumberKeyPressed(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) OnNumberKeyPressed(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) OnNumberKeyPressed(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) OnNumberKeyPressed(9);
        if (Input.GetKeyDown(KeyCode.Q)) OnQKeyPressed();
        if (Input.GetKeyDown(KeyCode.F1)) ToggleFullScreen();

    }

    private void OnEscapePressed() // Esc打开菜单
    {
        if (UIManager.Instance && UIManager.Instance.MenuButton && UIManager.Instance.MenuButton.activeSelf && UIManager.Instance.MenuButton.GetComponent<Button>().enabled)
        {
            UIManager.Instance.OpenMenu();
        }
        if (MenuSceneUIManager.Instance && MenuSceneUIManager.Instance.SettingsMenu) MenuSceneUIManager.Instance.setSettingsMenu(true);
    }

    private void OnSpacePressed() // 空格暂停
    {
        if (!UIManager.Instance) return;
        if (UIManager.Instance.Menu && UIManager.Instance.Menu.activeSelf) return;
        if (UIManager.Instance.PauseAndContinue && UIManager.Instance.PauseAndContinue.activeSelf && UIManager.Instance.PauseAndContinue.GetComponent<Button>().enabled)
        {
            UIManager.Instance.PauseAndContinue.GetComponent<PauseAndContinue>().onClick();
        }
    }

    private void OnNumberKeyPressed(int num) // 数字选卡
    {
        if (!CardManager.Instance) return;
        List<Card> cardList = CardManager.Instance.getCardList();
        if ((num + 9) % 10 >= cardList.Count) return;
        if (cardList[(num + 9) % 10].GetComponent<Button>().enabled == false) return;
        cardList[(num + 9) % 10].OnClick();
    }

    private void OnQKeyPressed() // Q键铲子
    {
        if (!CardManager.Instance || !CardManager.Instance.slotUI || !CardManager.Instance.slotUI.activeSelf) return;
        Shovel shovel = FindObjectOfType<Shovel>();
        if (shovel) shovel.OnClick();
    }

    private void ToggleFullScreen() // F1切换全屏
    {
        // 切换全屏状态，保持当前分辨率
        Screen.fullScreen = !Screen.fullScreen;

        Debug.Log($"全屏状态: {(Screen.fullScreen ? "全屏" : "窗口化")}");
        Debug.Log($"当前分辨率: {Screen.currentResolution.width}x{Screen.currentResolution.height}");
    }
}
