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

public class Shovel : MonoBehaviour
{
    public ShovelState state;
    private Plant target;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        target = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        setState(ShovelState.TobeUsed);
    }

    public void setState(ShovelState state)
    {
        if (this.state == state) return;
        this.state = state;
        switch (state)
        {
            case ShovelState.None:
                break;
            case ShovelState.TobeUsed:
                target = null;
                transform.position = CardManager.Instance.slotPlace.position;
                spriteRenderer.sortingLayerName = "CardList";
                break;
            case ShovelState.Suspension:
                spriteRenderer.sortingLayerName = "Hand";
                break;
            default:
                break;
        }
    }

    private void OnMouseDown()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant) target = collision.GetComponent<Plant>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant) target = null;
    }
}
