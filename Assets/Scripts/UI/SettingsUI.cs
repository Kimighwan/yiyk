using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        // SetGameVersion();
    }

    //private void SetGameVersion()
    //{
    //    gameVersionTxt.text = $"Version:{Application.version}";
    //}

    private void SetSoundSetting(bool sound)
    {
        soundOnToggle.SetActive(sound);
        soundOffToggle.SetActive(!sound);
    }

    public void OnClickSoundOnToggle()
    {
        //AudioManager.Instance.PlaySFX(SFX.ui_button_click);
    }

    public void OnClickSoundOffToggle()
    {
        //AudioManager.Instance.PlaySFX(SFX.ui_button_click);
    }


    public void OnClickSettingQuit()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf == true ? 0f : 1f;
    }

    public void ReStartBtn()
    {
        SceneLoader.Instance.ReloadScene();
    }
}
