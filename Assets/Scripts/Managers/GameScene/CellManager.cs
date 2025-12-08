using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public static CellManager Instance { get; private set; }

    public int maxRow, maxCol; // 从1开始

    public List<Cell> cellList;
    public List<GameObject> stripeList;

    private void Awake()
    {
        Instance = this;
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
            maxRow = Mathf.Max(maxRow, cell.row);
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

    public Cell getCell(int row, int col)
    {
        foreach (Cell cell in cellList)
        {
            if (cell.row == row && cell.col == col) return cell;
        }
        return null;
    }

    public void setState(GameState state)
    {
        switch (state)
        {
            case GameState.NotStarted:
                break;
            case GameState.Previewing:
                break;
            case GameState.SelectingCard:
                break;
            case GameState.Ready:
                setTombstones(); setIceTunnels();
                break;
            case GameState.Processing:
                break;
            case GameState.Paused:
                break;
            case GameState.Losing:
                break;
            case GameState.Winning:
                break;
            default:
                break;
        }
    }

    private bool setTombstone(int row, int col)
    {
        Cell cell = getCell(row, col);
        if (cell && !cell.tombstone)
        {
            cell.setCellProp(CellProp.Tombstone, true);
            return true;
        }
        return false;
    }

    private void setTombstones() // 生成坟墓
    {
        int tombstoneNum = GameManager.Instance.currLevelConfig.tombstoneNum, tombstoneArea = GameManager.Instance.currLevelConfig.tombstoneArea;
        if (tombstoneNum == 0) return;
        int col = (tombstoneArea + maxCol) % maxCol;
        if (tombstoneArea < 0) col += 1;
        int min_col = tombstoneArea > 0 ? 1 : col, max_col = tombstoneArea > 0 ? col : maxCol;
        int maxTombstoneNum = (max_col - min_col + 1) * maxRow;
        if (col > 0)
        {
            setTombstone(Random.Range(1, maxRow + 1), col); // 那一行至少生成一个
            tombstoneNum--; maxTombstoneNum--;
        }
        else
        {
            min_col = 1; max_col = maxCol;
            maxTombstoneNum = maxRow * maxCol;
        }
        while (tombstoneNum > 0 && maxTombstoneNum > 0)
        {
            bool flag = setTombstone(Random.Range(1, maxRow + 1), Random.Range(min_col, max_col + 1));
            if (flag)
            {
                tombstoneNum--; maxTombstoneNum--;
            }
        }
    }

    private void setIceTunnels() // 生成冰道
    {
        int iceTunnelArea = GameManager.Instance.currLevelConfig.iceTunnelArea;
        int col = (iceTunnelArea + maxCol) % maxCol;
        if (iceTunnelArea < 0) col += 1;
        int min_col = iceTunnelArea > 0 ? 1 : col, max_col = iceTunnelArea > 0 ? col : maxCol;
        if (col == 0)
        {
            min_col = 1; max_col = maxCol;
        }
        List<int> iceTunnelRows = GameManager.Instance.currLevelConfig.iceTunnelRows;
        foreach (Cell cell in cellList)
        {
            // 生成永久冰道
            if (iceTunnelRows.Contains(cell.row) && cell.col >= min_col && cell.col <= max_col) cell.setCellProp(CellProp.IceTunnel, true, -1);
        }
    }
}
