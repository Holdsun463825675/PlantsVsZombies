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
            awardPlantID = PlantID.GatlingPea,
            awardPropID = PropID.Shovel,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = false,
            startingSun = 8000,
            cardType = TypeOfCard.Fixation,
            fixedCards = new List<PlantID> { PlantID.BowlingWallNut, PlantID.BowlingRedWallNut, PlantID.BowlingBigWallNut },

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie},spawnWeight = 30,largeWave = true},
            },
            spawnTimer = 28.0f,
            restrictedArea = 0,
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
            nextLevelID = new List<int> { 5 },
            awardPlantID = PlantID.None,
            awardPropID = PropID.Shovel,
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
        });
        // 1-5
        levelConfigs.Add(new LevelConfig
        {
            levelID = 5,
            levelName = "1-5",
            nextLevelID = new List<int> { 8 },
            awardPlantID = PlantID.TallNut,
            awardPropID = PropID.None,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_loon,
            dropSun = false,
            startingSun = 0,
            cardType = TypeOfCard.Conveyor,
            fixedCards = new List<PlantID> { PlantID.BowlingWallNut, PlantID.BowlingWallNut, PlantID.BowlingWallNut, PlantID.BowlingWallNut, PlantID.BowlingRedWallNut },

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 15,largeWave = true},
            },
            shovel = false,
            spawnTimer = 30.0f,
            restrictedArea = 3,
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
            nextLevelID = new List<int> { 10001, 10002, 10003, 10004, 10005, 10006 },
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
            cardType = TypeOfCard.Fixation,
            fixedCards = new List<PlantID> { PlantID.PeaShooter, PlantID.Sunflower, PlantID.CherryBomb, PlantID.WallNut, PlantID.SnowPea, PlantID.TallNut },

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
            cardType = TypeOfCard.Fixation,
            fixedCards = new List<PlantID> { PlantID.PeaShooter, PlantID.Sunflower, PlantID.CherryBomb, PlantID.WallNut, PlantID.SnowPea, PlantID.Repeater, PlantID.Pumpkin },

            zombieID = new List<ZombieID> { ZombieID.BucketZombie, ZombieID.ScreenDoorZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ScreenDoorZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 10,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 20,largeWave = true},
            },
        });
        // 内测专属-足球快跑
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10003,
            levelName = "内测专属-足球快跑",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.GatlingPea,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_loon,
            dropSun = true,
            startingSun = 200,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 3,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 10,largeWave = true,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 20,largeWave = true,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 13,largeWave = false },
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.FootballZombie },spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie },spawnWeight = 30,largeWave = true,certainlySpawn=1},
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
            dropSun = false,
            startingSun = 0,
            cardType = TypeOfCard.Conveyor,
            fixedCards = new List<PlantID> { PlantID.PeaShooter, PlantID.CherryBomb, PlantID.WallNut, PlantID.SnowPea, PlantID.Repeater, PlantID.Torchwood, PlantID.TallNut, PlantID.Pumpkin, PlantID.GatlingPea },

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 4,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.NormalZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.ConeHeadZombie, ZombieID.NormalZombie},spawnWeight = 9,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 20,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ScreenDoorZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 12,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.ScreenDoorZombie},spawnWeight = 13,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 17,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 18,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 19,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 20,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 40,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 22,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 23,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 24,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 25,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 26,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 27,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 28,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 29,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 30,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.ScreenDoorZombie},spawnWeight = 60,largeWave = true},
            },
            spawnTimer = 28.0f,
        });
        // 内测专属-冰之歌
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10005,
            levelName = "内测专属-冰之歌",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.SnowPea,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Fixation,
            fixedCards = new List<PlantID> { PlantID.Sunflower, PlantID.CherryBomb, PlantID.WallNut, PlantID.SnowPea, PlantID.TallNut },

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.ConeHeadZombie, ZombieID.NormalZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.BucketZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.NormalZombie},spawnWeight = 10,largeWave = true,certainlySpawn=2},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.NormalZombie},spawnWeight = 20,largeWave = true,certainlySpawn=2},
            },
        });
        // 内测专属-绝对火力
        levelConfigs.Add(new LevelConfig
        {
            levelID = 10006,
            levelName = "内测专属-绝对火力",
            nextLevelID = new List<int> { },
            awardPlantID = PlantID.Torchwood,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Fixation,
            fixedCards = new List<PlantID> { PlantID.Sunflower, PlantID.WallNut, PlantID.Repeater, PlantID.Torchwood, PlantID.TallNut, PlantID.Pumpkin, PlantID.GatlingPea },

            zombieID = new List<ZombieID> { ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 1,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 2,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 3,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.BucketZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 4,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.BucketZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 10,largeWave = true,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 5,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie},spawnWeight = 6,largeWave = false,certainlySpawn=1},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 6,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 7,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 8,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 9,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 10,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> { ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 20,largeWave = true,certainlySpawn=2},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 11,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 12,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 13,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 14,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 15,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},spawnWeight = 16,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.BucketZombie, ZombieID.FootballZombie, ZombieID.NormalZombie, ZombieID.ConeHeadZombie},spawnWeight = 30,largeWave = true,certainlySpawn=2},
            },
        });
    }
}
