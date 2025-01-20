using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private void Awake()
    {
        SetStage();
    }

    public GameObject[] Stage;
    public void OnClickStageBtn(int stage) // 1 ~ 10
    {
        SceneLoader.Instance.LoadScene((SceneType)(stage + 3));
        AudioManager.Instance.PlayBGM(BGM.IngameBGM);
    }

    private void SetStage()
    {
        if(PlayerPrefs.GetInt("Stage") != 0)
        {
            for (int index = 1; index <= PlayerPrefs.GetInt("Stage"); index++)
            {
                var clearUI = Instantiate(Resources.Load<GameObject>("UI/Clear"));
                clearUI.transform.parent = Stage[index - 1].transform;
                clearUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(81, -71);
            }
        }

        for(int index = PlayerPrefs.GetInt("Stage") + 2; index <= 10; index++)
        {
            var lockUI = Instantiate(Resources.Load<GameObject>("UI/Lock"));
            lockUI.transform.parent = Stage[index - 1].transform;
            lockUI.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
