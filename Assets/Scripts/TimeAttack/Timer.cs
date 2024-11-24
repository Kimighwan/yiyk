using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float curTime = 0;
    public Text gameTimeText;

    private void Update()
    {
        curTime += Time.deltaTime;
        gameTimeText.text = "TIme : " + (int)curTime;
    }
}
