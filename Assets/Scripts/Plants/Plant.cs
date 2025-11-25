using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PlantID
{
    None,
    PeaShooter,
    Sunflower,
    CherryBomb
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
    Normal, Carrier, Surrounding, Flight
}

public class Plant : MonoBehaviour, IClickable
{
    private PlantState state = PlantState.None;
    public PlantID id = PlantID.None;
    public PlantType type = PlantType.Normal;

    private int maxHealth, currHealth;
    private TextMeshPro HPText;
    private Transform shadow;

    private Cell cell;

    private SpriteRenderer spriteRenderer;
    protected Animator anim;
    private Collider2D c2d;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        maxHealth = 300; currHealth = maxHealth;
        cell = null;
        anim = GetComponent<Animator>();
        Transform child = transform.Find("HPText");
        if (child) HPText = child.GetComponent<TextMeshPro>();
        if (HPText) HPText.gameObject.SetActive(false);
        shadow = transform.Find("Shadow");
        if (shadow) shadow.gameObject.SetActive(false);
        PlantManager.Instance.addPlant(this);
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
    public void Pause()
    {
        if (state != PlantState.Suspension) anim.enabled = false;
    }

    public void Continue()
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
                this.state = state;
                if (HPText) HPText.gameObject.SetActive(false);
                if (shadow) shadow.gameObject.SetActive(false);
                anim.enabled = false;
                c2d.enabled = false;
                if (cell) cell.setFlag(type, false);
                spriteRenderer.sortingLayerName = "Hand";
                break;
            case PlantState.Idle:
                this.state = state;
                if (HPText) HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.plantHealth);
                if (shadow) shadow.gameObject.SetActive(true);
                if (GameManager.Instance.state != GameState.Paused) anim.enabled = true;
                c2d.enabled = true;
                spriteRenderer.sortingLayerName = "Plant";
                break;
            case PlantState.Die:
                this.state = state;
                if (HPText) HPText.gameObject.SetActive(false);
                if (shadow) shadow.gameObject.SetActive(false);
                if (cell) cell.setFlag(type, false);
                Destroy(gameObject);
                spriteRenderer.sortingLayerName = "Plant";
                break;
            default:
                break;
        }
    }

    public void setCell(Cell cell)
    {
        this.cell = cell; 
    }

    public void AddHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0) setState(PlantState.Die);
    }

    public void UnderAttack(int point)
    {
        AddHealth(-point);
        anim.SetBool(AnimatorConfig.plant_underAttack, true);
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
