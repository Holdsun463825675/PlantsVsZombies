using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PlantID
{
    None = 0,
    PeaShooter = 1,
    Sunflower = 2,
    CherryBomb = 3,
    WallNut = 4,
    PotatoMine = 5,
    SnowPea = 6,
    Chomper = 7,
    Repeater = 8,
    Torchwood = 23,
    TallNut = 24,
    Pumpkin = 31,
    GatlingPea = 41,
    TwinSunflower = 42,
    BowlingWallNut = 49,
    BowlingRedWallNut = 50,
    BowlingBigWallNut = 51,
}

public enum PlantState
{
    None,
    Suspension,
    Idle,
    Die
}

public enum PlantType
{
    None, Normal, Carrier, Surrounding, Flight
}

public class Plant : MonoBehaviour, IClickable
{
    protected PlantState state = PlantState.None;
    public PlantID id = PlantID.None;
    public PlantType type = PlantType.Normal;
    public PlantID prePlantID = PlantID.None; // 种植的前置植物

    public int row;

    protected int maxHealth, currHealth;
    protected int underAttackSound; // 受击音效，0正常，1轻柔，2子弹
    protected TextMeshPro HPText;
    private Transform shadow;

    protected Cell cell;

    protected SpriteRenderer spriteRenderer;
    public Animator anim;
    private Collider2D c2d;

    protected string effectPlaceName = "EffectPlace";
    protected Transform effectPlace;
    protected Collider2D effectPlaceCollider;

    protected virtual void Awake()
    {
        row = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        maxHealth = 300; currHealth = maxHealth;
        underAttackSound = 0;
        cell = null;
        anim = GetComponent<Animator>();
        Transform child = transform.Find("HPText");
        if (child) HPText = child.GetComponent<TextMeshPro>();
        if (HPText) HPText.gameObject.SetActive(false);
        shadow = transform.Find("Shadow");
        if (shadow) shadow.gameObject.SetActive(false);
        PlantManager.Instance.addPlant(this);

        // 设置子物体碰撞器
        effectPlace = transform.Find(effectPlaceName);
        if (effectPlace)
        {
            effectPlaceCollider = effectPlace.GetComponent<Collider2D>();
            effectPlaceCollider.GetComponent<TriggerForwarder>().SetPlantParentHandler(this);
            effectPlaceCollider.enabled = false;
        }
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = true;
        setState(PlantState.Suspension);
    }

    void Update()
    {
        HPText.text = $"{currHealth}/{maxHealth}";
        if (state == PlantState.Suspension) HPText.gameObject.SetActive(false);
        else HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.plantHealth);
        if (GameManager.Instance.state == GameState.Paused ||
            GameManager.Instance.state == GameState.Losing ||
            GameManager.Instance.state == GameState.Winning) return;

        switch (state)
        {
            case PlantState.Suspension:
                SuspensionUpdate();
                break;
            case PlantState.Idle:
                IdleUpdate();
                break;
            case PlantState.Die:
                DieUpdate();
                break;
            default:
                break;
        }
    }

    // 暂停继续功能
    public virtual void Pause()
    {
        if (state != PlantState.Suspension) anim.enabled = false;
    }

    public virtual void Continue()
    {
        if (state != PlantState.Suspension) anim.enabled = true;
    }

    public void OnClick()
    {

    }

    public PlantState getState()
    {
        return state; 
    }

    public virtual void setState(PlantState state)
    {
        if (this.state == state) return;
        this.state = state;
        switch (state)
        {
            case PlantState.Suspension:
                row = 0;
                if (HPText) HPText.gameObject.SetActive(false);
                if (shadow) shadow.gameObject.SetActive(false);
                anim.enabled = false;
                c2d.enabled = false;
                if (effectPlaceCollider) effectPlaceCollider.enabled = false;
                if (cell) cell.removePlant(this);
                spriteRenderer.sortingLayerName = "Hand";
                break;
            case PlantState.Idle:
                if (HPText) HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.plantHealth);
                if (shadow) shadow.gameObject.SetActive(true);
                if (cell)
                {
                    cell.addPlant(this);
                    row = cell.row;
                } 
                if (GameManager.Instance.state != GameState.Paused) anim.enabled = true;
                c2d.enabled = true;
                if (effectPlaceCollider) effectPlaceCollider.enabled = true;
                spriteRenderer.sortingLayerName = "Plant";
                break;
            case PlantState.Die:
                row = 0;
                if (HPText) HPText.gameObject.SetActive(false);
                if (shadow) shadow.gameObject.SetActive(false);
                if (cell) cell.removePlant(this);
                if (effectPlaceCollider) effectPlaceCollider.enabled = false;
                spriteRenderer.sortingLayerName = "Plant";
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public void setCell(Cell cell)
    {
        this.cell = cell; 
    }

    public int setSortingOrder(int sortingOrder)
    {
        int count = 0;
        this.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder; count++;
        if (HPText) HPText.sortingOrder = sortingOrder + ++count; count++;
        return count;
    }

    protected virtual void AddHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0) setState(PlantState.Die);
    }

    protected void playUnderAttackSound()
    {
        switch (underAttackSound)
        {
            case (0):
                if (state == PlantState.Die) AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_gulp);
                else AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_chomps[Random.Range(0, ResourceConfig.sound_zombieeat_chomps.Length)]);
                break;
            case (1):
                if (state == PlantState.Die) AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_gulp);
                else AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_chompsoft);
                break;
            case (2):
                AudioManager.Instance.playClip(ResourceConfig.sound_bullethit_splats[Random.Range(0, ResourceConfig.sound_bullethit_splats.Length)]);
                break;
            default:
                break;
        }
    }

    public virtual void UnderAttack(int point, int type=0)
    {
        AddHealth(-point);
        anim.SetBool(AnimatorConfig.plant_underAttack, true);
        playUnderAttackSound();
    }

    private void SuspensionUpdate()
    {
        if (HPText) HPText.gameObject.SetActive(false);
    }

    protected virtual void IdleUpdate()
    {
        
    }

    protected virtual void DieUpdate()
    {
        if (HPText) HPText.gameObject.SetActive(false);
    }

    // 父物体处理触发事件的方法
    public virtual void OnChildTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void OnChildTriggerExit2D(Collider2D collision)
    {

    }
}
