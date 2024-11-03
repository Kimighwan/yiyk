using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
   public void LoadScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
