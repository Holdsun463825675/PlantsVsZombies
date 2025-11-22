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
            nextLevelID = 1,
            awardPlantID = PlantID.Sunflower,
            mapID = 3,
            time = TimeOfDay.Day,
            dropSun = true,
            startingSun = 1000,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {
                ZombieID.NormalZombie
            },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 100.0f,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 100.0f,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 100.0f,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 100.0f,largeWave = true},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 100.0f,largeWave = true},
            },
            spawnTimer = 28.0f,
        });

        // 1-1
        levelConfigs.Add(new LevelConfig
        {
            levelID = 1,
            levelName = "1-1",
            nextLevelID = 2,
            awardPlantID = PlantID.Sunflower,
            mapID = 1,
            time = TimeOfDay.Day,
            dropSun = true,
            startingSun = 150,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {
                ZombieID.NormalZombie
            },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
            }
        });

        // 1-2
        levelConfigs.Add(new LevelConfig
        {
            levelID = 2,
            levelName = "1-2",
            nextLevelID = 3,
            awardPlantID = PlantID.CherryBomb,
            mapID = 2,
            time = TimeOfDay.Day,
            dropSun = true,
            startingSun = 50,
            cardType = TypeOfCard.Autonomy,

            zombieID = new List<ZombieID> {
                ZombieID.NormalZombie
            },
            zombieWaves = new List<ZombieWave> {
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 1.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 2.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 3.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 4.0f,largeWave = false},
                new ZombieWave {zombieIDs = new List<ZombieID> {ZombieID.NormalZombie},spawnWeight = 5.0f,largeWave = true},
            }
        });
    }
}
