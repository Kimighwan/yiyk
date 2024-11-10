using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIManager : StartSceneCanvasManager
{
    public bool RDown = false;
    public float RDownTime = 0.0f;
    public bool RReStartDelay = false; // false : ����� ���� // ture : ��Ÿ��...

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
            Debug.Log("R Ű ������ ��");
            RDownTime += Time.deltaTime;
            if (RDownTime > 2.0f)  // R��ư 2�� ������ �����
            {
                SceneLoader.Instance.ReloadScene();
            }
        }

        if (!RDown) 
        {
            RDownTime = 0.0f;
        }
    }

    public void OnClickReStartBtn() // ����� ��ư ����
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
