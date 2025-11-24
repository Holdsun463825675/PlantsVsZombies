using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductHitSound
{
    None,
    Kernelpult,
    Butter,
    Melon
}

public class Product : MonoBehaviour
{
    public ProductHitSound hitSound;
    public int hitSoundPriority;

    protected virtual void Awake()
    {
        ProductManager.Instance.addProduct(this);
        hitSound = ProductHitSound.None; hitSoundPriority = 0;
    }

    public virtual void Pause()
    {

    }

    public virtual void Continue()
    {

    }
}
