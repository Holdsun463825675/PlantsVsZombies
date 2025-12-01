using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForwarder : MonoBehaviour, IClickable
{
    private Plant plantParentHandler;
    private Zombie zombieParentHandler;
    private Shovel shovelParentHandler;

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = false;
    }

    public void SetPlantParentHandler(Plant handler)
    {
        plantParentHandler = handler;
    }

    public void SetZombieParentHandler(Zombie handler)
    {
        zombieParentHandler = handler;
    }

    public void SetShovelParentHandler(Shovel handler)
    {
        shovelParentHandler = handler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        plantParentHandler?.OnChildTriggerEnter2D(collision);
        zombieParentHandler?.OnChildTriggerEnter2D(collision);
        shovelParentHandler?.OnChildTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        plantParentHandler?.OnChildTriggerExit2D(collision);
        zombieParentHandler?.OnChildTriggerExit2D(collision);
        shovelParentHandler?.OnChildTriggerExit2D(collision);
    }

    public void OnClick()
    {

    }
}
