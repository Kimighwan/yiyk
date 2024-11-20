using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public int idx = 0;
    public List<GameObject> cutSceneImg;

    public void OnClickNextCutScene()
    {
        if(idx < 6)
            cutSceneImg[idx].gameObject.SetActive(true);

        idx++;
        if(idx == 4)
        {
            cutSceneImg[0].gameObject.SetActive(false);
            cutSceneImg[1].gameObject.SetActive(false);
            cutSceneImg[2].gameObject.SetActive(false);
        }
        if(idx == 7)
        {
            SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, false, () =>
            {
                AudioManager.Instance.PlayBGM(BGM.IngameBGM);
                SceneLoader.Instance.LoadScene(SceneType.Stage1);
                SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, false);
            });
        }
    } 
}
