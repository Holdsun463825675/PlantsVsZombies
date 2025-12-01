using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        ProductManager.Instance.addProduct(this);
    }

    public virtual void Pause()
    {
        transform.DOPause();
        if (anim) anim.enabled = false;
    }

    public virtual void Continue()
    {
        transform.DOPlay();
        if (anim) anim.enabled = true;
    }
}
