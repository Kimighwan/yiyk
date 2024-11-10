using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : StartSceneCanvasManager
{
    public bool RDown = false;
    public float RDownTime = 0.0f;
    public bool RReStartDelay = false; // false : 재시작 가능 // ture : 쿨타임...

    private void Awake()
    {
        setting = transform.GetChild(0).gameObject;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        RDown = Input.GetKey(KeyCode.R); 
        if (RDown && !RReStartDelay)
        {
            Debug.Log("R 키 누르는 중");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R버튼 2초 누르면 재시작
            {
                SceneLoader.Instance.ReloadScene();
            }
        }

        if (!RDown) 
        {
            RDownTime = 0.0f;
        }
    }

    public void OnClickReStartBtn() // 재시작 버튼 구현
    {
        SceneLoader.Instance.ReloadScene();
    }
    

    private IEnumerator RReStartDelayCo() 
    {
        RReStartDelay = true;
        yield return new WaitForSeconds(10.0f);
        RReStartDelay = false;
    }
}
