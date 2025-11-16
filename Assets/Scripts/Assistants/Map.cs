using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapID;

    public List<Transform> dropSunPositions;
    public PolygonCollider2D zombiePreviewingPlace;
    public List<Transform> zombieSpawnPositions;
    public List<Transform> endlinePositions;
    public List<Transform> cameraPositions;
}
