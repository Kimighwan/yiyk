using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;        // 페이드 이미지
    public float fadeDuration = 5f; // 페이드 지속 시간

    private void Start()
    {
        // 게임 시작 시 페이드 인 효과
        StartCoroutine(FadeIn());
    }

    // 페이드 아웃 후 씬 재시작
    public void FadeOutAndRestart()
    {
        StartCoroutine(FadeOutAndReload());
    }

    // 페이드 아웃 효과 코루틴
    private IEnumerator FadeOutAndReload()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // 페이드 아웃
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        // 씬 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 씬이 다시 시작되면서 페이드 인
        StartCoroutine(FadeIn());
    }

    // 페이드 인 효과 코루틴
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // 페이드 인
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }
}
