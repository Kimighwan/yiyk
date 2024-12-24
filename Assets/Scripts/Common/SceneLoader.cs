using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    StartScene,
    CutScene,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6,
    Stage7,
    Stage8,
    Stage9,
    Stage10,
    Clear,
}

public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public Image fadeImg; // ÆäÀÌµå ÀÌ¹ÌÁö

    protected override void Init()
    {
        base.Init();

        fadeImg.transform.localPosition = Vector3.zero;
    }


    public void LoadScene(SceneType sceneType)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene(int idx = 0)
    {
        Fade(Color.black, 0f, 1f, 2.0f, 0f, false, () => // ¾îµÎ¿öÁü
        {
            LoadScene((SceneType)(SceneManager.GetActiveScene().buildIndex + idx));

            Fade(Color.black, 1f, 0f, 2.0f, 0f, false); // ¹à¾ÆÁü
        });

        Time.timeScale = 1f;
    }

    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }

    public void NextStage()
    {
        ReloadScene(1);
    }

    #region Fade

    public void Fade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactivateOnFinish, onFinish));
    }

    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, Action onFinish)
    {
        yield return new WaitForSeconds(startDelay);

        fadeImg.transform.localScale = Vector3.one;
        fadeImg.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            fadeImg.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        fadeImg.color = new Color(color.r, color.g, color.b, endAlpha);

        if (deactivateOnFinish)
            fadeImg.transform.localScale = Vector3.zero;

        onFinish?.Invoke();
    }

    #endregion
}
