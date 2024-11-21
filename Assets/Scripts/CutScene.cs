using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Button nextBtn;
    public List<GameObject> cutSceneImg;

    private int idx = 1;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.CutsceneBGM2);
    }

    public void OnClickNextCutScene()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

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
            nextBtn.interactable = false;
            SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, false, () =>
            {
                AudioManager.Instance.PlayBGM(BGM.IngameBGM);
                SceneLoader.Instance.LoadScene(SceneType.Test);
                SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, false);
            });
        }
    } 
}
