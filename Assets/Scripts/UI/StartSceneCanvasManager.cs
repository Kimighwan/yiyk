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
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ, ������� �ڷΰ��� ��ư ������
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if (setting.activeSelf) // UI�� ����� �ִٸ�
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
