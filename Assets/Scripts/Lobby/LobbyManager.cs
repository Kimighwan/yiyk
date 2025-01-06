using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public void OnClickStageBtn(int stage) // 1 ~ 10
    {
        SceneLoader.Instance.LoadScene((SceneType)(stage + 3));
    }
}
