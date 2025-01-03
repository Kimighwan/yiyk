using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Button skipBtn;
    public List<GameObject> cutSceneImg;

    private int idx = 1;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.CutsceneBGM2);
    }

    public void OnClickNextCutScene()
    {
        AudioManager.Instance.PlaySFX(SFX.CutsceneNext);

        if(idx < 6)
        {
            cutSceneImg[idx].gameObject.SetActive(true);
            StartCoroutine("CutActive");
        }

        idx++;
        if(idx == 4)
        {
            cutSceneImg[0].gameObject.SetActive(false);
            cutSceneImg[1].gameObject.SetActive(false);
            cutSceneImg[2].gameObject.SetActive(false);
        }
        if(idx == 7)
        {
            skipBtn.interactable = false;
            SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, false, () =>
            {
                AudioManager.Instance.PlayBGM(BGM.IngameBGM);
                SceneLoader.Instance.LoadScene(SceneType.Stage1);
                SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, false);
            });
        }
    }

    private IEnumerator CutActive()
    {
        SpriteRenderer spriteRenderer = cutSceneImg[idx].GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        float curTime = 0f;
        while(curTime < 1f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                Mathf.Lerp(0f, 1f, curTime / 1f));
            curTime += Time.deltaTime;
            yield return null;
        }
    }

    public void OnClickSkip()
    {
        skipBtn.interactable = false;
        SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, false, () =>
        {
            AudioManager.Instance.PlayBGM(BGM.IngameBGM);
            SceneLoader.Instance.LoadScene(SceneType.Stage1);
            SceneLoader.Instance.Fade(Color.black, 1f, 0f, 2.0f, 0f, false);
        });
    }
}
