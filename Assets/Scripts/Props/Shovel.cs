using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum ShovelState
{
    None,
    TobeUsed,
    Suspension,
}

public class Shovel : MonoBehaviour, IClickable
{
    public ShovelState state;
    private Plant target;
    private SpriteRenderer spriteRenderer;

    public string shovelPlaceName = "ShovelPlace";
    protected Collider2D shovelPlaceCollider;

    private void Awake()
    {
        target = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shovelPlaceCollider = transform.Find(shovelPlaceName).GetComponent<Collider2D>();
        shovelPlaceCollider.GetComponent<TriggerForwarder>().SetShovelParentHandler(this);
        shovelPlaceCollider.enabled = false;
    }

    private void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 30001;
        priority.isClickable = true;
        setState(ShovelState.TobeUsed);
    }

    public void setState(ShovelState state)
    {
        if (this.state == state) return;
        this.state = state;
        switch (state)
        {
            case ShovelState.None:
                shovelPlaceCollider.enabled = false;
                break;
            case ShovelState.TobeUsed:
                if (target)
                {
                    target.anim.SetBool(AnimatorConfig.plant_selected, false);
                    target = null;
                }
                transform.position = CardManager.Instance.slotPlace.position;
                shovelPlaceCollider.enabled = false;
                spriteRenderer.sortingLayerName = "CardList";
                break;
            case ShovelState.Suspension:
                shovelPlaceCollider.enabled = true;
                spriteRenderer.sortingLayerName = "Hand";
                break;
            default:
                break;
        }
    }

    public void OnClick()
    {
        switch (state)
        {
            case ShovelState.None:
                break;
            case ShovelState.TobeUsed:
                AudioManager.Instance.playClip(ResourceConfig.sound_buttonandputdown_shovel);
                setState(ShovelState.Suspension);
                HandManager.Instance.SuspendShovel(this);
                break;
            case ShovelState.Suspension:
                shovelPlant();
                HandManager.Instance.CancelShovel();
                break;
            default:
                break;
        }
    }

    private void shovelPlant()
    {
        if (target)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_placeplant_plant2);
            target.setState(PlantState.Die);
            target = null;
        }
    }

    // 父物体处理触发事件的方法
    public void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant)
        {
            if (target) target.anim.SetBool(AnimatorConfig.plant_selected, false);
            target = collision.GetComponent<Plant>();
            target.anim.SetBool(AnimatorConfig.plant_selected, true);
        }
    }

    public void OnChildTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant)
        {
            if (target)
            {
                target.anim.SetBool(AnimatorConfig.plant_selected, false);
                target = null;
            }
        }
    }
}
