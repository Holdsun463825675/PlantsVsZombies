using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSun : Sun
{
    protected override void Awake()
    {
        base.Awake();
        id = SunID.Medium;
        value = 25;
    }
}
