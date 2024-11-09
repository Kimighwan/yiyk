using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    // 타이틀
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

        while (!_asyncOperation.isDone) // 로딩이 진행 중일 때
        {
            LoadingSlider.value = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
            LoadingProgressTxt.text = $"{((int)(LoadingSlider.value * 100)).ToString()}%";

            // 로딩이 완료된다면 로비로 씬전환 후 코루틴 종료
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