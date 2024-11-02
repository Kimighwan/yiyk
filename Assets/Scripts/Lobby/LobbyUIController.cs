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
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ, ������� �ڷΰ��� ��ư ������
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            if (frontUI != null) // UI�� ����� �ִٸ�
            {
                frontUI.CloseUI(); // ������ִ� UI �ݱ�
            }
            else // �ƹ� UI�� ���ٸ� ���� ���� �˾�UI ����
            {
                //var uiData = new ConfirmUIData();
                //uiData.confirmType = ConfirmType.OK_CANCEL;
                //uiData.titleTxt = "Quit";
                //uiData.descTxt = "Do you want to quit game?";
                //uiData.okBtnTxt = "Quit";
                //uiData.cancelBtnTxt = "Cancel";
                //uiData.onClickOKBtn = () =>
                //{
                //    Application.Quit();
                //};
                //UIManager.Instance.OpenUI<ConfirmUI>(uiData);
                OnClickSettingsBtn();
            }
        }
    }

    public void OnClickSettingsBtn()
    {
        var settinsUI = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(settinsUI);
    }
}
