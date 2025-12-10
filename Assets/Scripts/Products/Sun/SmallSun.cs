using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSun : Sun
{
    protected override void Awake()
    {
        base.Awake();
        id = SunID.Small;
        value = 15;
    }
}
