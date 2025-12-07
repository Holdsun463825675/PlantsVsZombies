using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance { get; private set; }

    public int maxRow, maxCol; // ´Ó1¿ªÊ¼

    public List<Cell> cellList;
    public List<GameObject> stripeList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        maxRow = 0; maxCol = 0;
        cellList = new List<Cell>();
        stripeList = new List<GameObject>();
    }

    public void setMap(int restrictedArea = 0)
    {
        Transform parentCell = MapManager.Instance.currMap.cellList.transform;
        if (parentCell != null) foreach (Transform child in parentCell) cellList.Add(child.GetComponent<Cell>());
        Transform parentStripe = MapManager.Instance.currMap.stripeList.transform;
        if (parentStripe != null) foreach (Transform child in parentStripe) stripeList.Add(child.gameObject);
        foreach (Cell cell in cellList)
        {
            maxRow = Mathf.Max(maxCol, cell.col);
            maxCol = Mathf.Max(maxCol, cell.col);
        }
        setRestrictedArea(restrictedArea);
    }

    public void setRestrictedArea(int restrictedArea = 0)
    {
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
