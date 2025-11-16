using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    public void onCompleteAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.SetActive(false);
    }
}
