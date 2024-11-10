using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : StartSceneCanvasManager
{
    private void Awake()
    {
        setting = transform.GetChild(0).gameObject;
    }



    // 재시작 버튼 구현
}
