using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private void OnAnimCompleted()
    {
        Destroy(gameObject);
    }
}
