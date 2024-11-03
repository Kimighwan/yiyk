using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public void OnClickBtn()
    {
        SceneLoader.Instance.LoadScene(SceneType.StartScene);
    }
}
