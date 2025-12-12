using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float spawnTime = 30.0f;
    private float normalWaveDuration = 3.0f; // 小波延迟出怪时间
    private float hugeWaveDuration = 5.0f; // 大波延迟出怪时间
    private float waveDurationTimer = 0.0f;
    private bool isPlayingHugeWave = false;
    private float spawnTimer = 12.0f;
    private float healthPercentageThreshold = 0.6f;
    private int lastWaveZombieHealth = 0;
    private float currProcess = 0.0f;
    private float expectedProcess = 0.0f;
    private int currLargeWave = 0;

    private Vector2 lastDeadZombiePosition;
    private PolygonCollider2D zombiePreviewingPlace;
    private List<Transform> zombieSpawnPlaceList;

    private List<int> zombieNum; // 每一行的僵尸数量，控制每波出怪
    private List<Zombie> zombieList = new List<Zombie>();
    private List<Zombie> lastWaveZombieList = new List<Zombie>();

    private ZombieSpawnState state;
    private int[] orderInLayers;
    public int[] spawnProtection;

    private List<Zombie> zombiePreviewingList;
    private int currWaveNumber = 0;
    private float currWaveSurplusWeight = 0.0f;
    private List<ZombieID> zombieID; // 出现的僵尸
    private List<ZombieWave> zombieWaves; // 每波僵尸
    private List<ZombieID> specialZombies; // 特殊僵尸

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

    private void Update()
    {
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing || GameManager.Instance.state == GameState.Winning) return;
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
        zombieNum = new int[zombieSpawnPlaceList.Count].ToList();
        orderInLayers = new int[zombieSpawnPlaceList.Count];
        for (int i = 0; i < orderInLayers.Length; i++) orderInLayers[i] = i * rowMaxSortingOrder;
        spawnProtection = new int[zombieSpawnPlaceList.Count];
    }

    public void setConfig(
        List<ZombieID> zombieID, 
        List<ZombieWave> zombieWaves,
        List<ZombieID> specialZombies,
        float spawnMaxTime = 30.0f,
        float spawnTimer = 15.0f,
        float healthPercentageThreshold = 0.6f)
    {
        this.zombieID = zombieID;
        this.zombieWaves = zombieWaves;
        this.specialZombies = specialZombies;
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
                    foreach (Zombie zombie in PrefabSystem.Instance.zombiePrefabs)
                    {
                        if (zombie.zombieID == zombieID[i])
                        {
                            Zombie z = GameObject.Instantiate(zombie, randomPoint, Quaternion.identity).GetComponent<Zombie>();
                            z.setSortingOrder((int)(5.0f - z.transform.position.y * 1000));
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
        lastWaveZombieList.Remove(zombie);
        zombieList.Remove(zombie);
    }

    public void setState(ZombieSpawnState state)
    {
        this.state = state;
    }

    public List<Zombie> getZombieList()
    {
        return zombieList;
    }

    public Zombie getNearestZombie(Vector3 position, List<int> targetRows = null, List<Zombie> zombies = null)
    {
        float minDis = 999999;
        Zombie res = null;
        if (zombies == null) zombies = zombieList;
        foreach (Zombie zombie in zombies)
        {
            if (zombie == null || zombie.temptation) continue; // 跳过魅惑僵尸
            if (targetRows == null || targetRows.Contains(0) || targetRows.Contains(zombie.row))
            {
                float newDis = Vector3.Distance(position, zombie.transform.position);
                if (newDis < minDis)
                {
                    res = zombie; minDis = newDis;
                }
            }
        }
        return res;
    }

    public void killAllZombie()
    {
        foreach (Zombie zombie in zombieList) if (zombie && !zombie.temptation) zombie.kill();
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
        currProcess += speed * Time.deltaTime;
        if (currProcess > expectedProcess) currProcess = expectedProcess;
        UIManager.Instance.setLevelProcess(currProcess);
    }

    private void NotStartedUpdate()
    {

    }

    private void ProcessingUpdate()
    {
        if (currWaveNumber >= zombieWaves.Count) return;
        if (zombieList.Count > 0) lastDeadZombiePosition = zombieList[0].transform.position;

        
        spawnTimer += Time.deltaTime;
        levelProcessUpdate();

        getSpawnTime(); // 根据当前波僵尸血量调整出怪时间、出怪进度
        // 小波
        if (!zombieWaves[currWaveNumber].largeWave)
        {
            if (spawnTimer >= spawnTime)
            {
                spawnTimer = spawnTime;
                waveDurationTimer += Time.deltaTime;
                if (waveDurationTimer >= normalWaveDuration) nextWave();
            }
        }
        else // 大波
        {
            if (spawnTimer >= spawnMaxTime || currWaveNumber > 0 && getZombieHealthPercentage() == 0.0f) // 大波，达到最大出怪时间或场上僵尸全部死亡
            {
                spawnTimer = spawnMaxTime;
                if (currWaveNumber == 0) UIManager.Instance.activateLevelProcess();
                if (!isPlayingHugeWave)
                {
                    isPlayingHugeWave = true;
                    UIManager.Instance.playHugeWave();
                }
                waveDurationTimer += Time.deltaTime;
                if (waveDurationTimer >= hugeWaveDuration) nextWave();
            }
        }
    }

    private void EndUpdate()
    {
        spawnTimer += Time.deltaTime;
        getSpawnTime();
        levelProcessUpdate();
        if (HaveHealthyZombie()) lastDeadZombiePosition = zombieList[0].transform.position;
        else // 出怪结束且僵尸全部死亡
        {
            // TODO: 转成UI坐标
            //UIManager.Instance.WinAward.transform.position = lastDeadZombiePosition;
            GameManager.Instance.setState(GameState.Winning);
        }
    }

    private float getZombieHealthPercentage(List<Zombie> zombies=null)
    {
        if (zombies == null) zombies = lastWaveZombieList;
        int currZombieHealth = 0;
        foreach (Zombie zombie in zombies)
        {
            if (zombie && !zombie.temptation) currZombieHealth += zombie.getCurrHealth();
        }
        return (float)currZombieHealth / (float)lastWaveZombieHealth;
    }

    private bool HaveHealthyZombie(List<Zombie> zombies = null)
    {
        if (zombies == null) zombies = zombieList;
        foreach (Zombie zombie in zombies) if (zombie && zombie.isHealthy()) return true;
        return false;
    }


    private void getSpawnTime() // 根据当前波僵尸血量调整出怪时间
    {
        if (currWaveNumber > 0)
        {
            float currZombieHealthPercentage = getZombieHealthPercentage();
            if (currZombieHealthPercentage < healthPercentageThreshold)
            {
                spawnTime = spawnMaxTime * (currZombieHealthPercentage / healthPercentageThreshold);
            }
        }
    }

    private float getWaveMinZombieWeight()
    {
        float minWeight = 9999;
        foreach (ZombieID id in zombieWaves[currWaveNumber].zombieIDs)
        {
            foreach (Zombie zombie in PrefabSystem.Instance.zombiePrefabs)
            {
                if (zombie.zombieID != id) continue;
                minWeight = Mathf.Min(minWeight, zombie.spawnWeight);
            } 
        }
        return minWeight;
    }

    public int getMinZombieNumRow() // 获取当前出怪数量最少的行
    {
        if (zombieNum == null || zombieNum.Count == 0) return -1;

        float minValue = zombieNum.Min();
        var minIndices = zombieNum.Select((value, index) => new { value, index })
                            .Where(x => Mathf.Approximately(x.value, minValue))
                            .Select(x => x.index)
                            .ToList();

        return minIndices[Random.Range(0, minIndices.Count)];
    }

    private void spawnZombies()
    {
        if (currWaveNumber + 1 == zombieWaves.Count) // 最后一波
        {
            UIManager.Instance.playFinalWave();
            spawnSpecialZombies();
        } 
        zombieNum = new int[zombieSpawnPlaceList.Count].ToList();
        lastWaveZombieHealth = 0;
        lastWaveZombieList.Clear();
        if (zombieWaves[currWaveNumber].largeWave) spawnOneZombie(ZombieID.FlagZombie); // 大波生成旗帜僵尸
        for (int i = 0; i < zombieWaves[currWaveNumber].certainlySpawn; i++) spawnOneZombie(zombieWaves[currWaveNumber].zombieIDs[i]); // 必定生成的僵尸
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
            
            foreach (var go in PrefabSystem.Instance.zombiePrefabs)
            {
                if (go.GetComponent<Zombie>().zombieID == zombieID) zombiePrefab = go.GetComponent<Zombie>();
            }
            // 不符要求重新生成
            if (!zombiePrefab || zombiePrefab.spawnWeight > currWaveSurplusWeight && zombiePrefab.spawnWeight > getWaveMinZombieWeight()) return;
        }
        else
        {
            foreach (var go in PrefabSystem.Instance.zombiePrefabs)
            {
                if (go.GetComponent<Zombie>().zombieID == ID) zombiePrefab = go.GetComponent<Zombie>();
            }
            if (!zombiePrefab) return;
        }

        // 在僵尸数量最少的行生成
        int row = getMinZombieNumRow();
        zombieNum[row]++;
        if (spawnProtection[row] > 0)
        {
            spawnOneZombie(ID); // 有出怪保护则重新生成
            return;
        }

        Zombie zombie = GameObject.Instantiate(zombiePrefab, zombieSpawnPlaceList[row].position, Quaternion.identity);
        if (zombie)
        {
            row++;
            addZombie(zombie);
            currWaveSurplusWeight -= zombie.spawnWeight;
            lastWaveZombieHealth += zombie.getMaxHealth();
            lastWaveZombieList.Add(zombie);
            int count = zombie.setSortingOrder(orderInLayers[row - 1]);
            orderInLayers[row - 1] += count;
            zombie.setGameMode(row, CellManager.Instance.maxCol + 1); // 设置游戏模式
        }
    }

    private void spawnSpecialZombie(int row, int col) // 在特殊位置生成僵尸，不占出怪权重
    {
        if (specialZombies.Count == 0) return;

        bool flag = false; // 僵尸列表中是否包含特殊僵尸
        foreach (ZombieID id in specialZombies) if (zombieID.Contains(id)) flag = true;
        if (!flag) return; // 不包含则不生成

        //if (spawnProtection[row - 1] > 0) return; // 有出怪保护则不会出怪

        Zombie zombiePrefab = null;
        while (true)
        {
            // 随机僵尸
            ZombieID ID = specialZombies[Random.Range(0, specialZombies.Count)];
            foreach (var go in PrefabSystem.Instance.zombiePrefabs)
            {
                if (go.GetComponent<Zombie>().zombieID == ID) zombiePrefab = go.GetComponent<Zombie>();
            }
            if (zombieID.Contains(ID) && zombiePrefab) break;
        }
        Cell cell = CellManager.Instance.getCell(row, col);
        if (cell == null) return;
        Zombie zombie = GameObject.Instantiate(zombiePrefab, cell.transform.position, Quaternion.identity);
        if (zombie)
        {
            addZombie(zombie);
            lastWaveZombieHealth += zombie.getMaxHealth();
            lastWaveZombieList.Add(zombie);
            int count = zombie.setSortingOrder(orderInLayers[row - 1]);
            orderInLayers[row - 1] += count;
            zombie.setGameMode(row, col); // 设置游戏模式
        }
    }

    private void spawnSpecialZombies()
    {
        // 坟墓生成特殊僵尸
        foreach (Cell cell in CellManager.Instance.cellList)
        {
            if (cell.tombstone) spawnSpecialZombie(cell.row, cell.col);
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
            isPlayingHugeWave = false;
        }
        spawnZombies();
        currWaveNumber++;
        spawnTime = spawnMaxTime;
        spawnTimer = 0.0f;
        waveDurationTimer = 0.0f;
        if (currWaveNumber >= zombieWaves.Count) setState(ZombieSpawnState.End); // 结束出怪
    }
}
