using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSun : Sun
{
    protected override void Awake()
    {
        base.Awake();
        id = SunID.Large;
        value = 50;
    }
}
