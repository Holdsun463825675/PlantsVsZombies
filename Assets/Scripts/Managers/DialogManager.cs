using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public Dialog currDialog;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void createDialog(DialogType type, string text, System.Action action=null)
    {
        currDialog = null;
        Dialog prefab = null;
        switch (type)
        {
            case DialogType.Confirmation:
                prefab = PrefabSystem.Instance.dialogPrefabs[0];
                break;
            case DialogType.Message:
                prefab = PrefabSystem.Instance.dialogPrefabs[1];
                break;
            default:
                break;
        }
        if (prefab == null) return;
        currDialog = GameObject.Instantiate(prefab, FindCanvas().transform);
        currDialog.setText(text);
        if (action != null) currDialog.addConfirmAction(action);
    }

    Canvas FindCanvas()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        Canvas canvas = null;
        if (canvasObj != null)
        {
            canvas = canvasObj.GetComponent<Canvas>();
            if (canvas != null) return canvas;
        }
        if (canvasObj == null || canvas == null)
        {
            // 创建新的Canvas
            GameObject newCanvasObj = new GameObject("Canvas");
            canvas = newCanvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            newCanvasObj.AddComponent<CanvasScaler>();
            newCanvasObj.AddComponent<GraphicRaycaster>();
        }
        return canvas;
    }
}
