using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneCanvasManager : MonoBehaviour
{
    public GameObject settingUIPrefab;   // UI 설정찰 프리팹
    public GameObject settingBtnUI; // setting UI : 톱니바퀴
    public GameObject audioUIPrefab;
    public GameObject creditUI;

    public Button gameStartButton;

    public GameObject BackGroundFadeImg;
    public GameObject ketChapGaugeBackGround;

    private Canvas fadeCanvas;
    private bool clickSettingBtn = false;   // 세팅 버튼 클릭 여부
    public bool escException = false;      // 클릭후 esc로 설장창을 나갔는지 여부

    private void Awake()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade").GetComponent<Canvas>();
    }

    private void Start()
    {
        PlayerPrefs.SetFloat("BGMValue", 0.2f);
        PlayerPrefs.SetFloat("SFXValue", 0.5f);
        AudioManager.Instance.PlayBGM(BGM.MainBGM);
    }

    private void Update()
    {
        HandleInput();
    }

    protected virtual void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키, 모바일은 뒤로가기 버튼 누르면
        {
            escException = false;

            AudioManager.Instance.PlaySFX(SFX.ButtonClick);

            if (creditUI.activeSelf) // 만든이들 띄워져 있음
            {
                creditUI.SetActive(false);
            }
            else if (audioUIPrefab.activeSelf)
            {
                audioUIPrefab.SetActive(false);
                settingUIPrefab.SetActive(true);
            }
            else if (settingUIPrefab.activeSelf) // UI가 띄워져 있다면
            {
                settingBtnUI.SetActive(true);
                settingUIPrefab.SetActive(false);
                BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
                if (clickSettingBtn) escException = true;

                Time.timeScale = 1.0f;
            }
            else
            {
                OnClickSettingsBtn();
            }
        }
    }

    public void OnClickSettingsBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        settingUIPrefab.SetActive(!settingUIPrefab.activeSelf);
        settingBtnUI.SetActive(!settingBtnUI.activeSelf);
        BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
        Time.timeScale = settingUIPrefab.activeSelf == true ? 0f : 1f;  // 설정창 켜지면 시간 정지 : 꺼지면 시간 흐름
        //clickSettingBtn = settingUIPrefab.activeSelf == true ? true : false;    // 설정창 켜지면 T, 아니면 F
        escException = false;
    }

    public void OnClickQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        Application.Quit();
    }

    public void OnClickStartBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        fadeCanvas.sortingOrder = 1;

        gameStartButton.interactable = false;

        SceneLoader.Instance.Fade(Color.black, 0f, 1f, 2.0f, 0f, true, () =>
        {
            SceneManager.LoadScene("CutScene");
            SceneLoader.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, false);
            fadeCanvas.sortingOrder = 0;
        });
    }

    public void ClosedSetting()
    {
        settingUIPrefab.SetActive(false);
    }

    public void OnClickAudioBtn()   // 오디오 닫기 버튼에도 적용
    {
        audioUIPrefab.SetActive(!audioUIPrefab.activeSelf);
        settingUIPrefab.SetActive(!settingUIPrefab.activeSelf);
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        Time.timeScale = 0f;
    }
}
