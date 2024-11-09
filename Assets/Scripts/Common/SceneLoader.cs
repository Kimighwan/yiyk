using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    Title,
    StartScene,
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
    public Image fadeImg; // 페이드 이미지

    protected override void Init()
    {
        base.Init();

        fadeImg.transform.localPosition = Vector3.zero;
    }


    public void LoadScene(SceneType sceneType)
    {
        Time.timeScale = 1f;
        Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene()
    {
        SceneLoader.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        LoadScene((SceneType)(SceneManager.GetActiveScene().buildIndex + 1)); // 현재 씬의 인덱스 + 1로 다음 씬으로 이동
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
