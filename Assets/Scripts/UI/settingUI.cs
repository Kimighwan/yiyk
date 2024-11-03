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
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키, 모바일은 뒤로가기 버튼 누르면
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if (setting.activeSelf) // UI가 띄워져 있다면
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
