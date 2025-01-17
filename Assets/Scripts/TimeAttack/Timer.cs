using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float curTime = 0;
    public TextMeshProUGUI gameTimeText;

    public GameObject door;

    private void Update()
    {
        CheckTime();
    }

    private void CheckTime()
    {
        if (!door.activeSelf)
        {
            curTime += Time.deltaTime;
            gameTimeText.text = "TIme : " + curTime.ToString("F2");
        }
        else
        {
            PlayerPrefs.SetFloat("Time", curTime);
        }
    }
}
