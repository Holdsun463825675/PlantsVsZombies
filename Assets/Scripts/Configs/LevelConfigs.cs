using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigs
{
    public static List<LevelConfig> levelConfigs;

    static LevelConfigs()
    {
        levelConfigs = new List<LevelConfig>();
        InitializeDefaultLevels();
    }

    private static void InitializeDefaultLevels()
    {
        // 测试关
        levelConfigs.Add(new LevelConfig
        {
            levelID = 0,
            levelName = "Test",
            nextLevelID = new List<int> { 1 },
            awardPlantID = PlantID.Sunflower,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 1000,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 4,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 10,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 20,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 17,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 17,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 18,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 18,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 19,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 19,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 20,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 40,largeWave = true},
            },
            spawnTimer = 15.0f,
        });
        // 1-1
        levelConfigs.Add(new LevelConfig
        {
            levelID = 1,
            levelName = "1-1",
            nextLevelID = new List<int> { 2 },
            awardPlantID = PlantID.Sunflower,
            mapID = 1,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 150,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {ZombieID.NormalZombie},
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
            },
        });
        // 1-2
        levelConfigs.Add(new LevelConfig
        {
            levelID = 2,
            levelName = "1-2",
            nextLevelID = new List<int>{ 3 },
            awardPlantID = PlantID.CherryBomb,
            mapID = 2,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {ZombieID.NormalZombie},
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 6.0f,largeWave = true},
            },
        });
        // 1-3
        levelConfigs.Add(new LevelConfig
        {
            levelID = 3,
            levelName = "1-3",
            nextLevelID = new List<int> { 4 },
            awardPlantID = PlantID.WallNut,
            mapID = 2,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 8.0f,largeWave = true,certainlySpawn=1},
            },
        });
        // 1-4
        levelConfigs.Add(new LevelConfig
        {
            levelID = 4,
            levelName = "1-4",
            nextLevelID = new List<int> { 8 },
            awardPlantID = PlantID.None,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 5.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 10.0f,largeWave = true,certainlySpawn=1},
            },

            awardShovel = true,
        });
        // 1-5
        levelConfigs.Add(new LevelConfig
        {
            levelID = 5,
            levelName = "1-5",
        });
        // 1-6
        levelConfigs.Add(new LevelConfig
        {
            levelID = 6,
            levelName = "1-6",
        });
        // 1-7
        levelConfigs.Add(new LevelConfig
        {
            levelID = 7,
            levelName = "1-7",
        });
        // 1-8
        levelConfigs.Add(new LevelConfig
        {
            levelID = 8,
            levelName = "1-8",
            nextLevelID = new List<int> { 10001, 10002, 10003, 10004 },
            awardPlantID = PlantID.Repeater,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4.0f,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 10.0f,largeWave = true,certainlySpawn=1},
            },
        });
        // 1-9
        levelConfigs.Add(new LevelConfig
        {
            levelID = 9,
            levelName = "1-9",
        });
        // 1-10
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10,
            levelName = "1-10",
        });
        // 内测专属-此路不通
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10001,
            levelName = "内测专属-此路不通",
            nextLevelID = new List<int> {},
            awardPlantID = PlantID.TallNut,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.ConeHeadZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 10,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie},spawnWeight = 20,largeWave = true},
            },
        });
        // 内测专属-咣咣铛铛
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10002,
            levelName = "内测专属-咣咣铛铛",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.Pumpkin,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.BucketZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 10,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 20,largeWave = true},
            },
        });
        // 内测专属-足球快跑
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10003,
            levelName = "内测专属-足球快跑",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.GtalingPea,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_loon,
            dropSun = true,
            startingSun = 200,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 6,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 6,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 12,largeWave = true,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 7,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 8,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 10,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 12,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 25,largeWave = true,certainlySpawn=1},
            },
            spawnTimer = 25.0f,
            gameSpeed = 2.0f,
        });
        // 内测专属-大乱斗
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10004,
            levelName = "内测专属-大乱斗",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.TwinSunflower,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_ultimateBattle,
            dropSun = true,
            startingSun = 200,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 5,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 12,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 7,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 20,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 17,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 17,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 18,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 18,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 19,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 19,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 20,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 40,largeWave = true},
            },
            spawnTimer = 28.0f,
        });
    }
}
