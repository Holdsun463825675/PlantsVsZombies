using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig
{
    public int levelID; // 关卡ID
    public int nextLevelID; // 下一关
    public PlantID awardPlantID; // 通关奖励植物
    public string levelName; // 关卡名称
    public int mapID; // 地图
    public TimeOfDay time; // 时间
    public bool dropSun; // 是否掉落阳光
    public int startingSun; // 初始阳光
    public bool cleaner = true; // 是否有小推车

    public TypeOfCard cardType; // 选卡类型：自选卡0、固定选卡1、传送带2
    
    public List<ZombieID> zombieID; // 出现的僵尸
    public List<ZombieWave> zombieWaves; // 每波僵尸
    public float spawnMaxTime = 30.0f; // 每波最大间隔
    public float spawnTimer = 15.0f; // 初始波冷却
    public float healthPercentageThreshold = 0.6f; // 小波血量阈值
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
