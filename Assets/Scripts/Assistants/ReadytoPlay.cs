using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadytoPlay : MonoBehaviour
{
    public void onCompleteAnimation()
    {
        GameManager.Instance.setState(GameState.Processing);
    }
}
