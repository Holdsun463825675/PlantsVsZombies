using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForwarder : MonoBehaviour, IClickable
{
    private Plant parentHandler;

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = false;
    }

    public void SetParentHandler(Plant handler)
    {
        parentHandler = handler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        parentHandler?.OnChildTriggerEnter2D(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        parentHandler?.OnChildTriggerExit2D(collision);
    }

    public void OnClick()
    {

    }
}
