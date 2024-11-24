using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public TextMeshProUGUI resultTIme;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.ClearScene);
        resultTIme.text = "TIme : " + PlayerPrefs.GetFloat("Time").ToString("F3");
    }

    public void OnClickBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        SceneLoader.Instance.LoadScene(SceneType.StartScene);
    }
}
