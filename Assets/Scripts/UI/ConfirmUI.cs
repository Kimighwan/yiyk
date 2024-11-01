using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ConfirmType
{
    OK, // 단순히 알림성 팝업으로 특정 내용과 함께 확인 버튼만 있는 팝업
    OK_CANCEL, // 어떤 행위를 하려는 것이 맞는지 물어보는 팝업
}

public class ConfirmUIData : BaseUIData
{
    public ConfirmType confirmType;
    public string titleTxt; // 팝업 제목
    public string descTxt; // 팝업 내용
    public string okBtnTxt; // 확인 버튼 텍스트 // 굳이 필요한가? 상황에 달라 텍스트가 달라지기 때문에 만듦
    public Action onClickOKBtn; // 확인 눌렀을 때 실행하는 행위
    public string cancelBtnTxt; // 취소 버튼 텍스트
    public Action onClickCancelBtn; // 취소 눌렀을 때 실행하는 행위
}

public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI descTxt;
    public Button okBtn;
    public Button cancelBtn;
    public TextMeshProUGUI okBtnTxt;
    public TextMeshProUGUI cancelBtnTxt;

    private ConfirmUIData confirmUIData;
    private Action onClickOKBtn;
    private Action onClickCancelBtn;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        confirmUIData = uiData as ConfirmUIData;

        titleTxt.text = confirmUIData.titleTxt;
        descTxt.text = confirmUIData.descTxt;
        okBtnTxt.text = confirmUIData.okBtnTxt;
        cancelBtnTxt.text = confirmUIData.cancelBtnTxt;

        onClickOKBtn = confirmUIData.onClickOKBtn;
        onClickCancelBtn = confirmUIData.onClickCancelBtn;

        okBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(confirmUIData.confirmType == ConfirmType.OK_CANCEL);
    }

    public void OnClickOKBtn()
    {
        onClickOKBtn?.Invoke();
        onClickOKBtn = null;
        CloseUI();
    }

    public void OnClickCancelBtn()
    {
        onClickCancelBtn?.Invoke();
        onClickCancelBtn = null;
        CloseUI();
    }
}
