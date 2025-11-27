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
        // ≤‚ ‘πÿ
        levelConfigs.Add(new LevelConfig
        {
            levelID = 0,
            levelName = "Test",
            nextLevelID = new List<int>{ 1 },
            awardPlantID = PlantID.Sunflower,
            mapID = 3,
            time = TimeOfDay.Day,
            music = ResourceConfig.music_day,
            dropSun = true,
            startingSun = 1000,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {ZombieID.NormalZombie, ZombieID.ConeHeadZombie, ZombieID.BucketZombie, ZombieID.FootballZombie},
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
            nextLevelID = new List<int> { 5 },
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
    }
}
