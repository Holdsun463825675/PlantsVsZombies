using UnityEngine;
using System.Threading;

public class SingleInstanceSystem : MonoBehaviour
{
    private Mutex mutex;
    private bool createdNew;

    void Awake()
    {
        string mutexName = $"Global\\Holdsun_PlantsVsZombies";

        try
        {
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                Debug.LogError("游戏已经在运行中！请检查：");
                Debug.LogError("任务栏");
                Debug.LogError("系统托盘");
                Debug.LogError("任务管理器");
                Debug.LogError("正在退出新实例...");

                Application.Quit();
                return;
            }

            Debug.Log("第一个实例启动成功");
            DontDestroyOnLoad(gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Mutex创建失败: {e.Message}");
            // 即使失败也继续运行
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        if (mutex != null && createdNew)
        {
            mutex.ReleaseMutex();
            mutex.Close();
        }
    }

    // 提供给其他脚本的接口
    public static bool IsFirstInstance()
    {
        SingleInstanceSystem instance = FindObjectOfType<SingleInstanceSystem>();
        return instance != null && instance.createdNew;
    }
}