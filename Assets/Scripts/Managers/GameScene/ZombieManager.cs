using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ZombieSpawnState
{
    NotStarted,
    Processing,
    End
}

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }

    private int rowMaxSortingOrder = 5000;
    private float spawnMaxTime = 30.0f;
    private float spawnMinTime = 2.0f;
    private float spawnTime = 30.0f;
    private float hugeWaveDuration = 5.0f; // 大波延迟出怪时间
    private float hugeWaveDurationTimer = 0.0f;
    private bool isPlayingHugeWave = false;
    private float spawnTimer = 12.0f;
    private float healthPercentageThreshold = 0.5f;
    private int lastWaveZombieHealth = 0;
    private float currProcess = 0.0f;
    private float expectedProcess = 0.0f;
    private int currLargeWave = 0;

    private Vector2 lastDeadZombiePosition;
    private PolygonCollider2D zombiePreviewingPlace;
    private List<Transform> zombieSpawnPlaceList;
    
    private List<Zombie> zombieList = new List<Zombie>();
    private List<Zombie> lastWaveZombieList = new List<Zombie>();

    private ZombieSpawnState state;
    private int[] orderInLayers;
    public int[] spawnProtection;

    public List<Zombie> zombiePrefabList;

    private List<Zombie> zombiePreviewingList;
    private int currWaveNumber = 0;
    private float currWaveSurplusWeight = 0.0f;
    private List<ZombieID> zombieID; // 出现的僵尸
    private List<ZombieWave> zombieWaves; // 每波僵尸

    private void Awake()
    {
        Instance = this;
        spawnTime = spawnMaxTime;
        state = ZombieSpawnState.NotStarted;
        zombiePreviewingList = new List<Zombie>();
        zombieID = new List<ZombieID>();
        zombieWaves = new List<ZombieWave>();
        lastDeadZombiePosition = new Vector2();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;
        switch (state)
        {
            case ZombieSpawnState.NotStarted:
                NotStartedUpdate();
                break;
            case ZombieSpawnState.Processing:
                ProcessingUpdate();
                break;
            case ZombieSpawnState.End:
                EndUpdate();
                break;
            default:
                break;
        }
    }

    public void getMap()
    {
        zombiePreviewingPlace = MapManager.Instance.currMap.zombiePreviewingPlace;
        zombieSpawnPlaceList = MapManager.Instance.currMap.zombieSpawnPositions;
        orderInLayers = new int[zombieSpawnPlaceList.Count];
        for (int i = 0; i < orderInLayers.Length; i++) orderInLayers[i] = i * rowMaxSortingOrder;
        spawnProtection = new int[zombieSpawnPlaceList.Count];
    }

    public void setConfig(
        List<ZombieID> zombieID, 
        List<ZombieWave> zombieWaves, 
        float spawnMaxTime = 30.0f,
        float spawnTimer = 12.0f,
        float healthPercentageThreshold = 0.5f)
    {
        this.zombieID = zombieID;
        this.zombieWaves = zombieWaves;
        this.spawnMaxTime = spawnMaxTime;
        this.spawnTimer = spawnTimer;
        this.healthPercentageThreshold = healthPercentageThreshold;
    }

    public void setZombiePreviewing(bool flag=true)
    {
        if (flag == true)
        {
            Bounds bounds = zombiePreviewingPlace.bounds;
            for (int i = 0; i < zombieID.Count; i++)
            {
                Vector2 randomPoint = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
                if (zombiePreviewingPlace.OverlapPoint(randomPoint))
                {
                    foreach (Zombie zombie in zombiePrefabList)
                    {
                        if (zombie.zombieID == zombieID[i])
                        {
                            Zombie z = GameObject.Instantiate(zombie, randomPoint, Quaternion.identity).GetComponent<Zombie>();
                            zombiePreviewingList.Add(z);
                        }
                    }
                }
                else i--;
            }
        }
        else
        {
            for (int i = zombiePreviewingList.Count - 1; i >= 0; i--)
            {
                if (zombiePreviewingList[i] != null && zombiePreviewingList[i].gameObject != null) Destroy(zombiePreviewingList[i].gameObject);
            }
            zombiePreviewingList.Clear();
        }
    }

    // 暂停继续
    public void Pause()
    {
        foreach (Zombie zombie in zombieList) if (zombie) zombie.Pause();
    }

    public void Continue()
    {
        foreach (Zombie zombie in zombieList) if (zombie) zombie.Continue();
    }

    public void addZombie(Zombie zombie)
    {
        zombieList.Add(zombie);
    }

    public void removeZombie(Zombie zombie)
    {
        zombieList.Remove(zombie);
    }

    public void setState(ZombieSpawnState state)
    {
        this.state = state;
    }

    private void levelProcessUpdate()
    {
        // 根据上一波进行出怪进度计算
        // 小波
        if (currWaveNumber > 0 && !zombieWaves[currWaveNumber - 1].largeWave)
        {
            float currWavePercent = spawnTimer / spawnTime;
            expectedProcess = ((currWavePercent <= 1.0f ? currWavePercent : 1.0f) + (float)(currWaveNumber - 1)) / (float)zombieWaves.Count;
        }
        else // 大波
        {
            // 不动
        }
        // 出怪进度更新
        float speed = 1.0f / (float)zombieWaves.Count;
        currProcess += speed * Time.fixedDeltaTime;
        if (currProcess > expectedProcess) currProcess = expectedProcess;
        UIManager.Instance.setLevelProcess(currProcess);
    }

    private void NotStartedUpdate()
    {

    }

    private void ProcessingUpdate()
    {
        if (currWaveNumber >= zombieWaves.Count)
        {
            setState(ZombieSpawnState.End);
            return;
        } 
        if (zombieList.Count > 0) lastDeadZombiePosition = zombieList[0].transform.position;

        
        spawnTimer += Time.fixedDeltaTime;
        levelProcessUpdate();

        // 小波
        if (!zombieWaves[currWaveNumber].largeWave)
        {
            getSpawnTime(); // 根据当前波僵尸血量调整出怪时间
            if (spawnTimer >= spawnTime) nextWave();
        }
        else // 大波
        {
            // 下一波是大波时，不需要根据当前波僵尸血量调整出怪时间
            if (spawnTimer >= spawnTime || currWaveNumber > 0 && zombieList.Count == 0)
            {
                if (currWaveNumber == 0) UIManager.Instance.activateLevelProcess();
                if (!isPlayingHugeWave)
                {
                    isPlayingHugeWave = true;
                    UIManager.Instance.playHugeWave();
                }
                hugeWaveDurationTimer += Time.fixedDeltaTime;
                if (hugeWaveDurationTimer >= hugeWaveDuration) nextWave();
            }
        }
    }

    private void EndUpdate()
    {
        spawnTimer += Time.fixedDeltaTime;
        getSpawnTime();
        levelProcessUpdate();
        if (zombieList.Count > 0) lastDeadZombiePosition = zombieList[0].transform.position;
        else // 出怪结束且僵尸全部死亡
        {
            // TODO: 转成UI坐标
            //UIManager.Instance.WinAward.transform.position = lastDeadZombiePosition;
            GameManager.Instance.setState(GameState.Winning);
        }
    }

    private float getZombieHealthPercentage()
    {
        int currZombieHealth = 0;
        foreach (Zombie zombie in lastWaveZombieList)
        {
            if (zombie) currZombieHealth += zombie.getCurrHealth();
        }
        return (float)currZombieHealth / (float)lastWaveZombieHealth;
    }

    private void getSpawnTime() // 根据当前波僵尸血量调整出怪时间
    {
        if (currWaveNumber > 0)
        {
            float currZombieHealthPercentage = getZombieHealthPercentage();
            if (currZombieHealthPercentage < healthPercentageThreshold)
            {
                spawnTime = (spawnMaxTime - spawnMinTime) * (currZombieHealthPercentage / healthPercentageThreshold) + spawnMinTime;
            }
        }
    }

    private void spawnZombie()
    {
        if (currWaveNumber + 1 == zombieWaves.Count) UIManager.Instance.playFinalWave(); // 最后一波
        lastWaveZombieHealth = 0;
        lastWaveZombieList.Clear();
        while (currWaveSurplusWeight > 0.0f) spawnOneZombie();
    }

    private void spawnOneZombie(ZombieID ID=ZombieID.None)
    {
        // 获取预制体
        Zombie zombiePrefab = null;
        if (ID == ZombieID.None)
        {
            // 随机僵尸
            ZombieID zombieID = zombieWaves[currWaveNumber].zombieIDs[Random.Range(0, zombieWaves[currWaveNumber].zombieIDs.Count)];
            
            foreach (var go in zombiePrefabList)
            {
                if (go.GetComponent<Zombie>().zombieID == zombieID) zombiePrefab = go.GetComponent<Zombie>();
            }
            // 不符要求重新生成
            if (!zombiePrefab || zombiePrefab.spawnWeight > currWaveSurplusWeight && zombiePrefab.spawnWeight > 1.0f) return;
        }
        else
        {
            foreach (var go in zombiePrefabList)
            {
                if (go.GetComponent<Zombie>().zombieID == ID) zombiePrefab = go.GetComponent<Zombie>();
            }
            if (!zombiePrefab) return;
        }

        // 在某一行生成
        int row = 0;
        while (true)
        {
            // 随机行
            row = Random.Range(0, zombieSpawnPlaceList.Count);
            if (spawnProtection[row] == 0) break;
        }

        Zombie zombie = GameObject.Instantiate(zombiePrefab, zombieSpawnPlaceList[row].position, Quaternion.identity);
        if (zombie)
        {
            addZombie(zombie);
            currWaveSurplusWeight -= zombie.spawnWeight;
            lastWaveZombieHealth += zombie.getMaxHealth();
            lastWaveZombieList.Add(zombie);
            int count = zombie.setSortingOrder(orderInLayers[row]);
            orderInLayers[row] += count;
            zombie.setGameMode(); // 设置游戏模式
        }
    }

    private void nextWave()
    {
        // 更新出怪保护
        bool feasible = false;
        while (!feasible)
        {
            for (int i = 0; i < spawnProtection.Length; i++)
            {
                if (spawnProtection[i] > 0) spawnProtection[i]--;
                if (spawnProtection[i] == 0) feasible = true;
            }
        }

        currWaveSurplusWeight = zombieWaves[currWaveNumber].spawnWeight * SettingSystem.Instance.settingsData.spawnMultiplier; // 根据难度设置出怪权重
        if (!zombieWaves[currWaveNumber].largeWave) // 小波
        {
            expectedProcess = (float)currWaveNumber / (float)zombieWaves.Count;
            if (currWaveNumber == 0)
            {
                AudioManager.Instance.playClip(ResourceConfig.sound_textsound_awooga);
                UIManager.Instance.activateLevelProcess();
            }
        }
        else // 大波
        {
            expectedProcess = (float)(currWaveNumber + 1) / (float)zombieWaves.Count; // 出怪进度大走一步
            AudioManager.Instance.playClip(ResourceConfig.sound_textsound_siren);
            UIManager.Instance.Flags[currLargeWave++].GetComponent<Animator>().enabled = true;
            hugeWaveDurationTimer = 0.0f;
            isPlayingHugeWave = false;
            spawnOneZombie(ZombieID.FlagZombie); // 生成旗帜僵尸
        }
        spawnZombie();
        currWaveNumber++;
        spawnTime = spawnMaxTime;
        spawnTimer = 0.0f;
    }
}
