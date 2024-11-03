using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SingletonBehaviour<FadeManager>
{
    public Image fadeImage;        // ���̵� �̹���
    public float fadeDuration = 3f; // ���̵� ���� �ð�
    private StageManager stageManager;
    protected override void Init()
    {
        base.Init();
    }

    private void Awake()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    private void Start()
    {
        // ���� ���� �� ���̵� �� ȿ��
        StartCoroutine(FadeIn());
    }

    // ���̵� �ƿ� �� �� �����
    public void FadeOutAndRestart()
    {
        StartCoroutine(FadeOutAndReload());
    }

    // ���̵� �ƿ� ȿ�� �ڷ�ƾ
    private IEnumerator FadeOutAndReload()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // ���̵� �ƿ�
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            Time.timeScale = 0;
            fadeImage.color = color;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        stageManager.ActivateStage(stageManager.currentStageIndex);

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
        Time.timeScale = 1;
    }
}