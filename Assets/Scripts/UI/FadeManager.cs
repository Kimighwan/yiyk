using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SingletonBehaviour<FadeManager>
{
    public Image fadeImage;        // ���̵� �̹���
    public float fadeDuration = 5f; // ���̵� ���� �ð�

    protected override void Init()
    {
        base.Init();
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
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        // �� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // ���� �ٽ� ���۵Ǹ鼭 ���̵� ��
        StartCoroutine(FadeIn());
    }

    // ���̵� �� ȿ�� �ڷ�ƾ
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        // ���̵� ��
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
