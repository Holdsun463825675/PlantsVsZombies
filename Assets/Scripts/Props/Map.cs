using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int mapID;
    private string cellListName = "CellList";
    private string stripeListName = "StripeList";
    public int maxRow, maxCol; // ´Ó1¿ªÊ¼

    public List<Cell> cellList;
    public List<GameObject> stripeList;
    public List<Transform> dropSunPositions;
    public PolygonCollider2D zombiePreviewingPlace;
    public List<Transform> zombieSpawnPositions;
    public List<Transform> endlinePositions;
    public List<Transform> cameraPositions;
    public List<Cleaner> cleaners;
    public List<Transform> cleanerPositions_begin;
    public List<Transform> cleanerPositions_end;

    private void Awake()
    {
        maxRow = 0; maxCol = 0;
        cellList = new List<Cell>();
        stripeList = new List<GameObject>();
        Transform parentCell = transform.Find(cellListName);
        if (parentCell != null) foreach (Transform child in parentCell) cellList.Add(child.GetComponent<Cell>());
        Transform parentStripe = transform.Find(stripeListName);
        if (parentStripe != null) foreach (Transform child in parentStripe) stripeList.Add(child.gameObject);
    }

    public void setRestrictedArea(int restrictedArea=0)
    {
        foreach (Cell cell in cellList)
        {
            maxRow = Mathf.Max(maxCol, cell.col);
            maxCol = Mathf.Max(maxCol, cell.col);
        } 
        int stripeIdx = (restrictedArea + maxCol) % maxCol;
        for (int i = 0; i < stripeList.Count; i++)
        {
            if (i == stripeIdx) stripeList[i].SetActive(true);
            else stripeList[i].SetActive(false);
        }
        foreach (Cell cell in cellList)
        {
            if (restrictedArea > 0)
            {
                if (cell.col <= stripeIdx) cell.gameObject.SetActive(true);
                else cell.gameObject.SetActive(false);
            }
            else
            {
                if (cell.col > stripeIdx) cell.gameObject.SetActive(true);
                else cell.gameObject.SetActive(false);
            }
        }
    }

}
