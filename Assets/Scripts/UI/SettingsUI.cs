using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    public Button reStartButton;

    public GameObject BackGroundFadeImg;
    public GameObject ketChapGaugeBackGround;
    public GameObject settingButnUI;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        
    }

    //private void SetGameVersion()
    //{
    //    gameVersionTxt.text = $"Version:{Application.version}";
    //}

    public void OnClickSettingQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        gameObject.SetActive(!gameObject.activeSelf);
        BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
        Time.timeScale = gameObject.activeSelf == true ? 0f : 1f;
    }

    public void ReStartBtn()
    {
        reStartButton.interactable = false;
        OnClickSettingQuit();
        SceneLoader.Instance.ReloadScene();
    }

    // URL 링크 열기
    public void OnClickOpenURL(string url)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        Application.OpenURL(url);
    }

    public void OnClickLobby()
    {
        OnClickSettingQuit();

        AudioManager.Instance.StopBGM();

        SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, true, () =>
        {            
            SceneLoader.Instance.LoadScene(SceneType.Lobby);
            SceneLoader.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, false);
            AudioManager.Instance.PlayBGM(BGM.Lobby);
        });
    }
    public void OnClickMain()
    {
        OnClickSettingQuit();

        AudioManager.Instance.StopBGM();

        SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, true, () =>
        {
            SceneLoader.Instance.LoadScene(SceneType.StartScene);
            SceneLoader.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, false);
            AudioManager.Instance.PlayBGM(BGM.MainBGM);
        });
    }
}