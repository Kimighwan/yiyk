using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.ClearScene);
    }

    public void OnClickBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        SceneLoader.Instance.LoadScene(SceneType.StartScene);
    }
}
