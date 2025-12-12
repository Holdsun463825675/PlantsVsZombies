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
    public const int Bowling_layer = 20;
    public const int Cleaner_layer = 21;

    private void Awake()
    {
        SetupCustomLayers();
        Time.fixedDeltaTime = 0.04f;
        Physics2D.velocityIterations = 3;
        Physics2D.positionIterations = 1;
        Physics2D.autoSyncTransforms = false; // 很重要！
    }

    public static void SetupCustomLayers()
    {
        SetupCollisionMatrix();
    }

    static void SetupCollisionMatrix()
    {
        Physics2D.IgnoreLayerCollision(Plant_layer, Plant_layer, true); // 植物不与植物碰撞
        Physics2D.IgnoreLayerCollision(Plant_layer, Bullet_layer, true); // 植物不与子弹碰撞
        Physics2D.IgnoreLayerCollision(Plant_layer, Sun_layer, true); // 植物不与阳光碰撞
        Physics2D.IgnoreLayerCollision(Plant_layer, Cleaner_layer, true); // 植物不与小推车碰撞

        Physics2D.IgnoreLayerCollision(Zombie_layer, Zombie_layer, true); // 僵尸不与僵尸碰撞
        Physics2D.IgnoreLayerCollision(Zombie_layer, Cell_layer, true); // 僵尸不与格子碰撞

        Physics2D.IgnoreLayerCollision(Bullet_layer, Bullet_layer, true); // 子弹不与子弹碰撞
        Physics2D.IgnoreLayerCollision(Bullet_layer, Sun_layer, true); // 子弹不与阳光碰撞
        Physics2D.IgnoreLayerCollision(Bullet_layer, Cleaner_layer, true); // 子弹不与小推车碰撞

        Physics2D.IgnoreLayerCollision(Sun_layer, Sun_layer, true); // 阳光不与阳光碰撞
        Physics2D.IgnoreLayerCollision(Sun_layer, Cleaner_layer, true); // 阳光不与小推车碰撞

        Physics2D.IgnoreLayerCollision(AttackPlace_layer, Shovel_layer, true); // 铲子不与植物攻击范围碰撞
        Physics2D.IgnoreLayerCollision(Bowling_layer, Default_layer, true); // 保龄球不与默认碰撞
        Physics2D.IgnoreLayerCollision(Bowling_layer, Plant_layer, true); // 保龄球不与植物碰撞
        //Physics2D.IgnoreLayerCollision(Bowling_layer, Zombie_layer, true); // 保龄球不与僵尸碰撞

        Physics2D.IgnoreLayerCollision(Zombie_layer, Plant_layer, false); // 僵尸与植物碰撞
    }
}
