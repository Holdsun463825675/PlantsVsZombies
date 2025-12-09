using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Pause()
    {
        if (anim) anim.enabled = false;
    }

    public void Continue()
    {
        if (anim) anim.enabled = true;
    }


    private void OnAnimCompleted()
    {
        Destroy(gameObject);
    }
}
