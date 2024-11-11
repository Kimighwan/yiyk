using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager :  StartSceneCanvasManager
{
    [SerializeField] private bool RDown = false; // R 키를 눌렀는가? // true: 누르는 중
    [SerializeField] private float RDownTime = 0.0f; // R 키를 몇초 눌렀는가?
    [SerializeField] private bool reLoadingScene = false; // ReloadScene이 계속 요청되지 않도록 예외 처리용 변수
    [SerializeField] private bool collTime = false; // R 키 재시작 기능의 쿨타임 체크용 // 아래 coolTimeCo 코루틴 확인 

    private void Awake()
    {
        setting = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        StartCoroutine("coolTimeCo");
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        RDown = Input.GetKey(KeyCode.R); 
        if (RDown && collTime && !reLoadingScene)
        {
            Debug.Log("R 키 누르는 중");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R버튼 2초 누르면 재시작
            {
                reLoadingScene = true; // ReloadScene이 계속 요청되지 않도록 예외 처리
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

    private IEnumerator coolTimeCo() // 씬이 시작되고 5초가 지나야 재시작 가능
    {
        yield return new WaitForSeconds(5.0f);
        collTime = true;
    }

    public void OnClickReStartBtn() // 재시작 버튼 구현
    {
        SceneLoader.Instance.ReloadScene();
    }
}
