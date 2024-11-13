using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvasManager : MonoBehaviour
{
    public GameObject setting;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.MainBGM);
    }

    private void Update()
    {
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키, 모바일은 뒤로가기 버튼 누르면
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if (setting.activeSelf) // UI가 띄워져 있다면
            {
                setting.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                OnClickSettingsBtn();
            }
        }
    }

    public void OnClickSettingsBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        setting.SetActive(!setting.activeSelf);
        Time.timeScale = setting.activeSelf == true ? 0f : 1f;
    }

    public void OnClickQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        Application.Quit();
    }

    public void OnClickStartBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        SceneManager.LoadScene("Title");
    }
}
