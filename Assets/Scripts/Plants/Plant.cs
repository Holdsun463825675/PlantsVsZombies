using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlantID
{
    PeaShooter,
    Sunflower
}

public enum PlantState
{
    Suspension,
    Idle,
    Effect,
    Die
}

public enum PlantType
{
    Normal, Carrier, Surrounding, Flight
}

public class Plant : MonoBehaviour, IClickable
{
    public string shootPlaceColliderName = "ShootPlace";

    private PlantState state;
    public PlantID id = PlantID.PeaShooter;
    public PlantType type = PlantType.Normal;

    private int maxHealth, currHealth;
    private TextMeshPro HPText;
    private Transform shadow;

    private Cell cell;

    private Animator anim;
    private Collider2D c2d;
    private Collider2D shootPlaceCollider;

    protected List<Zombie> targets;

    protected virtual void Awake()
    {
        c2d = GetComponent<Collider2D>();
        maxHealth = 300; currHealth = maxHealth;
        cell = null; targets = new List<Zombie>();
        anim = GetComponent<Animator>();
        Transform child = transform.Find("HPText");
        if (child) HPText = child.GetComponent<TextMeshPro>();
        if (HPText) HPText.gameObject.SetActive(false);
        shadow = transform.Find("Shadow");
        if (shadow) shadow.gameObject.SetActive(false);

        // 设置子物体碰撞器
        child = transform.Find(shootPlaceColliderName);
        if (child) shootPlaceCollider = child.GetComponent<Collider2D>();
        if (shootPlaceCollider)
        {
            shootPlaceCollider.GetComponent<TriggerForwarder>().SetParentHandler(this);
            shootPlaceCollider.enabled = false;
        }
        PlantManager.Instance.addPlant(this);
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = true;
        setState(PlantState.Suspension);
    }

    void FixedUpdate()
    {
        HPText.text = $"{currHealth}/{maxHealth}";
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
            case PlantState.Effect:
                EffectUpdate();
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

    public void setState(PlantState state)
    {
        if (state == PlantState.Suspension)
        {
            this.state = state;
            if (HPText) HPText.gameObject.SetActive(false);
            if (shadow) shadow.gameObject.SetActive(false);
            if (shootPlaceCollider) shootPlaceCollider.enabled = false;
            anim.enabled = false;
            c2d.enabled = false;
            if (cell) cell.setFlag(type, false);
        }
        if (state == PlantState.Idle)
        {
            this.state = state;
            if (HPText) HPText.gameObject.SetActive(GameManager.Instance.plantHealth);
            if (shadow) shadow.gameObject.SetActive(true);
            if (shootPlaceCollider) shootPlaceCollider.enabled = true;
            if (GameManager.Instance.state != GameState.Paused) anim.enabled = true;
            c2d.enabled = true;
        }
        if (state == PlantState.Effect)
        {
            if (HPText) HPText.gameObject.SetActive(GameManager.Instance.plantHealth);
            if (shadow) shadow.gameObject.SetActive(true);
            if (shootPlaceCollider) shootPlaceCollider.enabled = true;
            anim.SetTrigger(AnimatorConfig.plant_isEffect);
            c2d.enabled = true;
        }
        if (state == PlantState.Die)
        {
            this.state = state;
            if (HPText) HPText.gameObject.SetActive(false);
            if (shadow) shadow.gameObject.SetActive(false);
            if (shootPlaceCollider) shootPlaceCollider.enabled = false;
            if (cell) cell.setFlag(type, false);
            Destroy(gameObject);
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
        if (HPText) HPText.gameObject.SetActive(GameManager.Instance.plantHealth);
    }

    protected virtual void EffectUpdate()
    {
        if (HPText) HPText.gameObject.SetActive(GameManager.Instance.plantHealth);
    }

    protected virtual void DieUpdate()
    {
        if (HPText) HPText.gameObject.SetActive(false);
    }

    // 父物体处理触发事件的方法
    public void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            Zombie target = collision.GetComponent<Zombie>();
            if (target && !targets.Contains(target)) targets.Add(target);
        }
    }

    public void OnChildTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.zombie)
        {
            Zombie target = collision.GetComponent<Zombie>();
            if (target) targets.Remove(target);
        }
    }
}
