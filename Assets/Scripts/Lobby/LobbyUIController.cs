using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    public void Init()
    {
        
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키, 모바일은 뒤로가기 버튼 누르면
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            if (frontUI != null) // UI가 띄워져 있다면
            {
                frontUI.CloseUI(); // 띄워져있는 UI 닫기
            }
            else // 아무 UI도 없다면 게임 종료 팝업UI 띄우기
            {
                var uiData = new ConfirmUIData();
                uiData.confirmType = ConfirmType.OK_CANCEL;
                uiData.titleTxt = "Quit";
                uiData.descTxt = "Do you want to quit game?";
                uiData.okBtnTxt = "Quit";
                uiData.cancelBtnTxt = "Cancel";
                uiData.onClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
    }

    public void OnClickSettingsBtn()
    {

        var settinsUI = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(settinsUI);
    }
}
