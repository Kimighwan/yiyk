using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager :  StartSceneCanvasManager
{
    [SerializeField] private bool RDown = false; // R Ű�� �����°�? // true: ������ ��
    [SerializeField] private float RDownTime = 0.0f; // R Ű�� ���� �����°�?
    [SerializeField] private bool reLoadingScene = false; // ReloadScene�� ��� ��û���� �ʵ��� ���� ó���� ����
    [SerializeField] private bool collTime = false; // R Ű ����� ����� ��Ÿ�� üũ�� // �Ʒ� coolTimeCo �ڷ�ƾ Ȯ�� 

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
            Debug.Log("R Ű ������ ��");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R��ư 2�� ������ �����
            {
                reLoadingScene = true; // ReloadScene�� ��� ��û���� �ʵ��� ���� ó��
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

    private IEnumerator coolTimeCo() // ���� ���۵ǰ� 5�ʰ� ������ ����� ����
    {
        yield return new WaitForSeconds(5.0f);
        collTime = true;
    }

    public void OnClickReStartBtn() // ����� ��ư ����
    {
        SceneLoader.Instance.ReloadScene();
    }
}
