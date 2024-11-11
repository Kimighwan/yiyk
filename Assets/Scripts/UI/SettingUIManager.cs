using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : StartSceneCanvasManager
{
    public bool RDown = false; // R 키를 눌렀는가? // true: 누르는 중
    public float RDownTime = 0.0f; // R 키를 몇초 눌렀는가?

    private void Awake()
    {
        setting = transform.GetChild(0).gameObject;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        RDown = Input.GetKey(KeyCode.R); 
        if (RDown)
        {
            Debug.Log("R 키 누르는 중");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R버튼 2초 누르면 재시작
            {
                SceneLoader.Instance.ReloadScene();
            }
        }
        else // 누른 시간 초기화
        {
            RDownTime = 0.0f;
        }

        //if (!RDown)  // 누른 시간 초기화
        //{
        //    RDownTime = 0.0f;
        //}
    }

    public void OnClickReStartBtn() // 재시작 버튼 구현
    {
        SceneLoader.Instance.ReloadScene();
    }
}
