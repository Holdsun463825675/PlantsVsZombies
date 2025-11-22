using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingConfig
{
    public static readonly Dictionary<float, float> gameSpeedMap = new Dictionary<float, float>
    {
        {0.0f, 0.1f},
        {1.0f, 0.5f},
        {2.0f, 1.0f},
        {3.0f, 2.0f},
        {4.0f, 3.0f},
    };

    public static readonly Dictionary<float, float> spawnMultiplierMap = new Dictionary<float, float>
    {
        {0.0f, 0.5f},
        {1.0f, 1.0f},
        {2.0f, 1.5f},
        {3.0f, 2.0f},
        {4.0f, 3.0f},
        {5.0f, 4.0f},
    };

    public static readonly Dictionary<float, float> hurtRateMap = new Dictionary<float, float>
    {
        {0.0f, 2.0f},
        {1.0f, 1.5f},
        {2.0f, 1.0f},
        {3.0f, 0.75f},
        {4.0f, 0.50f},
        {5.0f, 0.25f},
    };
}
