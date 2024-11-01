using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIData
{
    public Action OnShow;
    public Action OnClose;
}

public class BaseUI : MonoBehaviour
{
    public Animation uiAnim;

    public Action uiOnShow;
    public Action uiOnClose;

    public virtual void Init(Transform anchor)
    {

        uiOnShow = null;
        uiOnClose = null;

        transform.SetParent(anchor);

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;
    }

    public virtual void SetInfo(BaseUIData uiData)
    {

        uiOnShow = uiData.OnShow;
        uiOnClose = uiData.OnClose;
    }

    public virtual void ShowUI()
    {
        if (uiAnim != null)
        {
            uiAnim.Play();
        }

        uiOnShow?.Invoke();
        uiOnShow = null;
    }

    public virtual void CloseUI(bool isCloseAll = false)
    {
        if (!isCloseAll)
        {
            uiOnClose?.Invoke();
        }
        uiOnClose = null;

        UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
