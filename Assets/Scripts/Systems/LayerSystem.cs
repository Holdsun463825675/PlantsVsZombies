using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LayerSystem : MonoBehaviour
{
    public const int Default_layer = 0;
    public const int UI_layer = 5;
    public const int Background_layer = 11;
    public const int Plant_layer = 12;
    public const int Zombie_layer = 13;
    public const int AttackPlace_layer = 14;
    public const int Cell_layer = 15;
    public const int Shovel_layer = 16;
    public const int Bullet_layer = 17;
    public const int Sun_layer = 18;
    public const int Card_layer = 19;

    private void Awake()
    {
        SetupCustomLayers();
    }


    [MenuItem("PVZ/Setup Layers")]
    public static void SetupCustomLayers()
    {
        SetupCollisionMatrix();
    }

    static void SetupCollisionMatrix()
    {
        Physics2D.IgnoreLayerCollision(Bullet_layer, Bullet_layer, true); // ×Óµ¯²»Óë×Óµ¯Åö×²
        Physics2D.IgnoreLayerCollision(AttackPlace_layer, Shovel_layer, true); // ²ù×Ó²»ÓëÖ²Îï¹¥»÷·¶Î§Åö×²
        Physics2D.IgnoreLayerCollision(AttackPlace_layer, Zombie_layer, false); // Ö²Îï¹¥»÷·¶Î§Óë½©Ê¬Åö×²
        Physics2D.IgnoreLayerCollision(AttackPlace_layer, Default_layer, false); // Ö²Îï¹¥»÷·¶Î§Óë½©Ê¬Åö×²
    }
}
