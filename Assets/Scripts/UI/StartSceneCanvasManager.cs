using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneCanvasManager : MonoBehaviour
{
    public GameObject setting;
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ, ������� �ڷΰ��� ��ư ������
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if (setting.activeSelf) // UI�� ����� �ִٸ�
            {
                setting.SetActive(false);
            }
            else
            {
                OnClickSettingsBtn();
            }
        }
    }

    public void OnClickSettingsBtn()
    {
        setting.SetActive(!setting.activeSelf);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("Title");
    }
}