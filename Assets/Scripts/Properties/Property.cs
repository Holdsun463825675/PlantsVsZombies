using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : MonoBehaviour
{
    protected virtual void Awake()
    {
        PropertyManager.Instance.addProperty(this);
    }

    public virtual void Pause()
    {

    }

    public virtual void Continue()
    {

    }
}
