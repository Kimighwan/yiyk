using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : SingletonBehaviour<FadeManager>
{
    // public Image fadeImg;        // ���̵� �̹���

    //public float fadeDuration = 5f; // ���̵� ���� �ð�
    //private StageManager stageManager;

    protected override void Init()
    {
        base.Init();
        // fadeImg.transform.localPosition = Vector3.zero;
    }

    //private void Awake()
    //{
    //    stageManager = FindObjectOfType<StageManager>();
    //}

    //private void Start()
    //{
    //    // ���� ���� �� ���̵� �� ȿ��
    //    StartCoroutine(FadeIn());
    //}

    //// ���̵� �ƿ� �� �� �����
    //public void FadeOutAndRestart()
    //{
    //    StartCoroutine(FadeOutAndReload());
    //}

    //// ���̵� �ƿ� ȿ�� �ڷ�ƾ
    //private IEnumerator FadeOutAndReload()
    //{
    //    float elapsedTime = 0f;
    //    Color color = fadeImg.color;

    //    // ���̵� �ƿ�
    //    while (elapsedTime < fadeDuration)
    //    {
    //        color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
    //        Time.timeScale = 0;
    //        fadeImg.color = color;
    //        elapsedTime += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    color.a = 1;
    //    fadeImg.color = color;

    //    stageManager.ActivateStage(stageManager.currentStageIndex);

    //    StartCoroutine(FadeIn());
    //}

    //private IEnumerator FadeIn()
    //{
    //    float elapsedTime = 0f;
    //    Color color = fadeImg.color;

    //    while (elapsedTime < fadeDuration)
    //    {
    //        color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
    //        fadeImg.color = color;
    //        elapsedTime += Time.unscaledDeltaTime;
    //        yield return null;
    //    }

    //    color.a = 0;
    //    fadeImg.color = color;
    //    Time.timeScale = 1;
    //}
}