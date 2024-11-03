using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUI : MonoBehaviour
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
}
