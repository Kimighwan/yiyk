using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : StartSceneCanvasManager
{
    public bool RDown = false; // R Ű�� �����°�? // true: ������ ��
    public float RDownTime = 0.0f; // R Ű�� ���� �����°�?

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
            Debug.Log("R Ű ������ ��");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R��ư 2�� ������ �����
            {
                SceneLoader.Instance.ReloadScene();
            }
        }
        else // ���� �ð� �ʱ�ȭ
        {
            RDownTime = 0.0f;
        }

        //if (!RDown)  // ���� �ð� �ʱ�ȭ
        //{
        //    RDownTime = 0.0f;
        //}
    }

    public void OnClickReStartBtn() // ����� ��ư ����
    {
        SceneLoader.Instance.ReloadScene();
    }
}
