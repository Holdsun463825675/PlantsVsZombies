using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapID;

    public GameObject cellList;
    public GameObject stripeList;
    public List<Transform> dropSunPositions;
    public PolygonCollider2D zombiePreviewingPlace;
    public List<Transform> zombieSpawnPositions;
    public List<Transform> endlinePositions;
    public List<Transform> cameraPositions;
    public List<Cleaner> cleaners;
    public List<Transform> cleanerPositions_begin;
    public List<Transform> cleanerPositions_end;

}
