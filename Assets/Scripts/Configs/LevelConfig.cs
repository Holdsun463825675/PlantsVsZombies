using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    public int levelID; // 关卡ID
    public List<int> nextLevelID = new List<int>(); // 通关后解锁的关卡
    public PlantID awardPlantID = PlantID.None; // 通关奖励植物
    public PropID awardPropID = PropID.None; // 通关奖励道具
    public string levelName; // 关卡名称
    public int mapID; // 地图
    public TimeOfDay time; // 时间
    public string music; // 背景音乐
    public bool dropSun; // 是否掉落阳光
    public int startingSun; // 初始阳光

    public TypeOfCard cardType; // 选卡类型
    public List<PlantID> fixedCards = new List<PlantID>(); // 固定选卡/传送带卡
    
    public List<ZombieID> zombieID; // 出现的僵尸
    public List<ZombieWave> zombieWaves; // 每波僵尸
    public float spawnMaxTime = 30.0f; // 每波最大间隔
    public float spawnTimer = 15.0f; // 初始波冷却
    public float healthPercentageThreshold = 0.6f; // 小波血量阈值

    // 其他
    public bool cleaner = true; // 是否有小推车
    public bool shovel = true; // 是否可用铲子
    public List<ZombieID> specialZombies = new List<ZombieID>(); // 特殊出怪
    public int tombstoneNum = 0, tombstoneArea = 0; // 坟墓数量、坟墓出现区域（与可种植区域一样）
    public int restrictedArea = 0; // 可种植区域，0为全可种植，大于0为靠近家的可种，小于0为靠近出怪位置可种
    public float generateCardTime = 6.0f; // 传送带出生成植物间隔
    public float gameSpeed = -1; // 固定游戏速度
    public bool cardCoolingDown = true; // 卡片是否有冷却
}

public enum TimeOfDay
{
    Day = 0,
    Night = 1
}

public enum TypeOfCard
{
    Autonomy = 0,
    Fixation = 1,
    Conveyor = 2
}

public class ZombieWave
{
    public List<ZombieID> zombieIDs; // 会出现的僵尸
    public float spawnWeight; // 出怪权重
    public bool largeWave; // 是否为大波
    public int certainlySpawn = 0; // 必定出现的前几种僵尸
}
