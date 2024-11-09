using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // Ÿ��Ʋ
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgressTxt;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        // LogoAnim.gameObject.SetActive(true);
        Title.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadGameCo());
    }

    private IEnumerator LoadGameCo()
    {
        Title.SetActive(true); 

        _asyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.Stage1);
        if (_asyncOperation == null)
        {
            yield break;
        }

        _asyncOperation.allowSceneActivation = false;

        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";
        yield return new WaitForSeconds(0.5f);

        while (!_asyncOperation.isDone) // �ε��� ���� ���� ��
        {
            LoadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";

            // �ε��� �Ϸ�ȴٸ� �κ�� ����ȯ �� �ڷ�ƾ ����
            if (_asyncOperation.progress >= 0.9f)
            {
                SceneLoader.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, () =>
                {
                    _asyncOperation.allowSceneActivation = true;
                });
                yield break;
            }
            yield return null;
        }
    }
}