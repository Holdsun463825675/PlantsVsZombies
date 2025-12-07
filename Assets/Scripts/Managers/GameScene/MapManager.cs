using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public Map currMap;

    public List<Map> MapList;

    private void Awake()
    {
        Instance = this;
    }

    public void setMap(int mapID = 0)
    {
        foreach (Map map in MapList)
        {
            if (map.mapID == mapID)
            {
                currMap = map;
                map.gameObject.SetActive(true);
            } 
            else map.gameObject.SetActive(false);
        }
    }
}
