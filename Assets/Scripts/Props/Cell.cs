using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum CellType
{
    None = 0, Grass = 1, Pool = 2, Roof = 3,
}

public enum CellProp
{
    None = 0, IceTunnel = 1, Crater = 2, Tombstone = 3,
}

public class Cell : MonoBehaviour, IClickable
{
    private int rowMaxSortingOrder = 5000, colMaxSortingOrder = 500;
    public int row, col;
    public CellType cellType = CellType.None;
    public Dictionary<PlantType, List<Plant>> plants = new Dictionary<PlantType, List<Plant>>();
    private Plant virtualPlant;

    public bool iceTunnel, crater, tombstone; // 是否有冰道、弹坑、坟墓
    public float craterTime, craterTimer;
    public float iceTunnelTime, iceTunnelTimer;
    public float tombStoneTime, tombStoneTimer;

    public Collider2D c2d;

    public GameObject IceTunnel;
    public List<GameObject> Craters;
    public List<GameObject> Tombstones;
    public List<GameObject> TombstoneMounds;

    private void Awake()
    {
        c2d = GetComponent<Collider2D>();
        craterTime = 120.0f; iceTunnelTime = 60.0f; tombStoneTime = -1; // 默认持续时间，-1为永久
        setCellProp(CellProp.IceTunnel, false);
        setCellProp(CellProp.Crater, false);
        setCellProp(CellProp.Tombstone, false);
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30000;
        priority.isClickable = true;
    }

    private void Update()
    {
        if (!HandManager.Instance.currPlant) unsetVirtualPlant();

        if (crater && GameManager.Instance.state == GameState.Processing) // 计时
        {
            IceTunnelUpdate();
            CraterUpdate();
            TombstoneUpdate();
        }
    }

    private void OnMouseEnter()
    {
        setVirtualPlant(HandManager.Instance.currPlant);
    }

    private void OnMouseExit()
    {
        unsetVirtualPlant();
    }

    public void setCellProp(CellProp prop, bool flag, float time=0)
    {
        switch (prop)
        {
            case CellProp.IceTunnel:
                if (flag == true)
                {
                    iceTunnel = true;
                    if (time != 0) iceTunnelTimer = time;
                    else iceTunnelTimer = iceTunnelTime;
                    IceTunnel.SetActive(true);
                    killAllPlants();
                }
                else
                {
                    iceTunnel= false;
                    iceTunnelTimer = iceTunnelTime;
                    IceTunnel.SetActive(false);
                }
                break;
            case CellProp.Crater:
                if (flag == true)
                {
                    crater = true;
                    if (time != 0) craterTimer = time;
                    else craterTimer = craterTime;
                    craterTimer = 0.0f;
                    Craters[0].SetActive(true);
                    killAllPlants();
                }
                else
                {
                    crater = false;
                    craterTimer = craterTime;
                    foreach (GameObject go in Craters) go.SetActive(false);
                }
                break;
            case CellProp.Tombstone:
                if (flag == true)
                {
                    tombstone = true;
                    if (time != 0) tombStoneTimer = time;
                    else tombStoneTimer = tombStoneTime;
                    int idx = Random.Range(0, Tombstones.Count);
                    for (int i = 0; i < Tombstones.Count; i++)
                    {
                        if (i == idx)
                        {
                            Tombstones[i].SetActive(true); TombstoneMounds[i].SetActive(true);
                        }
                        else
                        {
                            Tombstones[i].SetActive(false); TombstoneMounds[i].SetActive(false);
                        }
                    }
                    killAllPlants();
                }
                else
                {
                    tombstone = false;
                    tombStoneTimer = tombStoneTime;
                    foreach (GameObject go in Tombstones) go.SetActive(false);
                    foreach (GameObject go in TombstoneMounds) go.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        HandManager.Instance.PlantPlant(this);
    }

    public void addPlant(Plant plant)
    {
        if (plant.type == PlantType.None) return;
        if (!plants.ContainsKey(plant.type)) plants.Add(plant.type, new List<Plant>());
        if (!plants[plant.type].Contains(plant)) plants[plant.type].Add(plant);
    }

    public void removePlant(Plant plant)
    {
        if (plant.type == PlantType.None) return;
        if (!plants.ContainsKey(plant.type)) return;
        plants[plant.type].Remove(plant);
    }

    private Plant getPlant(PlantID id)
    {
        foreach (PlantType plantType in plants.Keys)
        {
            foreach (Plant pl in plants[plantType])
            {
                if (pl.id == id) return pl;
            }
        }
        return null;
    }

    // 判断是否可种植植物
    public bool PlantAvailable(Plant plant)
    {
        if (cellType == CellType.None) return false;

        // 重叠判定
        if (plant.type == PlantType.None) return true;
        if (plant.prePlantID == PlantID.None) // 不需要前置植物，需判断格子类型
        {
            if (plants.ContainsKey(plant.type) && plants[plant.type].Count > 0) return false;
            if (!plant.cellTypes.Contains(cellType) && !plant.cellTypes.Contains(CellType.None))
            {
                if (!plants.ContainsKey(PlantType.Carrier) || plants[PlantType.Carrier].Count == 0) return false;
            }
        }
        else // 需要前置植物，只需判断前置植物
        {
            Plant prePlant = getPlant(plant.prePlantID);
            if (prePlant == null) return false;
        }

        // 特殊植物判定
        switch (plant.id)
        {
            case PlantID.PotatoMine:
                if (cellType == CellType.Pool) return false;
                break;
            case PlantID.GraveBuster: // 墓碑吞噬者
                if (tombstone) return true;
                else return false;
            case PlantID.Coffeebean: // 咖啡豆
                foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
                {
                    foreach (Plant pl in pair.Value) if (pl && pl.sleep) return true;
                }
                return false;
            default:
                break;
        }

        // 常规植物判定
        if (iceTunnel || crater || tombstone) return false;

        return true;
    }

    private Vector3 plantPlace(Plant plant)
    {
        Vector3 place = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // 特殊植物判定
        switch (plant.id)
        {
            case PlantID.GraveBuster: // 墓碑吞噬者
                place.y += 1.0f;
                return place;
            default:
                break;
        }

        float baseY = 0.0f, offset = 0.0f, carrier_offset = 0.0f;
        // 有载体时抬高
        if (plants.ContainsKey(PlantType.Carrier) && plants[PlantType.Carrier].Count > 0)
        {
            switch (cellType)
            {
                case CellType.Grass:
                    carrier_offset = 0.3f;
                    break;
                case CellType.Pool:
                    carrier_offset = 0.15f;
                    break;
                case CellType.Roof:
                    baseY = 0.2f; carrier_offset = 0.3f;
                    break;
                default:
                    break;
            }
        }
        switch (plant.type)
        {
            case PlantType.Normal:
                offset = 0.2f;
                break;
            case PlantType.Carrier:
                offset = 0.0f;
                break;
            case PlantType.Surrounding:
                offset = 0.0f;
                break;
            case PlantType.Flight:
                offset = 1.0f;
                break;
            default:
                break;
        }
        place.y = transform.position.y + baseY + offset;
        if (plant.type != PlantType.Carrier) place.y += carrier_offset;
        return place;
    }

    private int getPlantSortingOrder(Plant plant)
    {
        int res = row * rowMaxSortingOrder - col * colMaxSortingOrder;
        switch (plant.type)
        {
            case PlantType.None:
                res += 8;
                break;
            case PlantType.Carrier:
                res += 4;
                break;
            case PlantType.Surrounding:
                res += 12;
                break;
            case PlantType.Normal:
                res += 8;
                break;
            case PlantType.Flight:
                res += 16;
                break;
            default:
                break;
        }
        return res;
    }

    private void setVirtualPlant(Plant plant)
    {
        if (plant == null) return;
        if (!PlantAvailable(plant)) return;
        Plant virtualPlantPrefab = PrefabSystem.Instance.GetPlantPrefab(plant.id);
        virtualPlant = GameObject.Instantiate(virtualPlantPrefab);
        virtualPlant.setSortingOrder(virtualPlant.GetComponent<SpriteRenderer>().sortingOrder - 1);
        virtualPlant.transform.position = plantPlace(virtualPlant);
        // 调整透明度
        Color currentColor = virtualPlant.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 127f / 255f);
        virtualPlant.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void unsetVirtualPlant()
    {
        if (virtualPlant)
        {
            Destroy(virtualPlant.gameObject);
            virtualPlant = null;
        }
    }

    public bool PlantPlant(Plant plant)
    {
        if (plant == null) return false;
        if (!PlantAvailable(plant)) return false;

        if (plant.prePlantID != PlantID.None) // 需要前置植物
        {
            getPlant(plant.prePlantID).kill(); // 替换前置植物
        }

        addPlant(plant);
        plant.setSortingOrder(getPlantSortingOrder(plant));
        adjustPlantPlace();
        return true;
    }

    public void adjustPlantPlace()
    {
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            PlantType plantType = pair.Key;
            foreach (Plant plant in pair.Value) if (plant) plant.transform.position = plantPlace(plant);
        }
    }

    public bool canShovel(Plant plant) // 是否可以铲植物
    {
        if (plant.type != PlantType.Carrier) return true;
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            PlantType plantType = pair.Key;
            if (plantType == PlantType.Carrier) continue;
            foreach (Plant pl in pair.Value) if (!pl.cellTypes.Contains(cellType)) return false;
        }
        return true;
    }

    public void AllPlantsUnderAttack(int attackPoint, int dieMode = 0)
    {
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            foreach (Plant pl in pair.Value) if (!pl.cellTypes.Contains(cellType)) pl.UnderAttack(attackPoint, dieMode);
        }
    }

    public void killAllPlants(int dieMode=0) // 击杀该格子所有植物
    {
        foreach (KeyValuePair<PlantType, List<Plant>> pair in plants)
        {
            foreach (Plant pl in pair.Value) if (!pl.cellTypes.Contains(cellType)) pl.kill(dieMode);
        }
    }

    private void IceTunnelUpdate()
    {
        if (iceTunnelTimer == -1) return;
        iceTunnelTimer -= Time.deltaTime;
        if (iceTunnelTimer <= 0) setCellProp(CellProp.IceTunnel, false);
    }

    private void CraterUpdate()
    {
        if (craterTimer == -1)
        {
            Craters[0].SetActive(true);
            return;
        } 
        craterTimer -= Time.deltaTime;
        if (craterTimer <= 0) setCellProp(CellProp.Crater, false);
        else
        {
            int idx = (int)((craterTime - craterTimer) / craterTime * Craters.Count);
            for (int i = 0; i < Craters.Count; i++)
            {
                if (i == idx) Craters[i].SetActive(true);
                else Craters[i].SetActive(false);
            }
        }
    }

    private void TombstoneUpdate()
    {
        if (tombStoneTimer == -1) return;
        tombStoneTimer -= Time.deltaTime;
        if (tombStoneTimer <= 0) setCellProp(CellProp.Tombstone, false);
    }
}
