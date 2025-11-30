using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    protected virtual void Awake()
    {
        ProductManager.Instance.addProduct(this);
    }

    public virtual void Pause()
    {

    }

    public virtual void Continue()
    {

    }
}
