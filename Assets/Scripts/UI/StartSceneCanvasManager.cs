using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneCanvasManager : MonoBehaviour
{
    public GameObject setting;   // UI ������ ������
    public GameObject settingUI; // setting UI : ��Ϲ���
    public Button gameStartButton;

    public Canvas fadeCanvas;

    public GameObject BackGroundFadeImg;
    public GameObject maker;
    public GameObject ketChapGaugeBackGround;

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
        if (Input.GetKeyDown(KeyCode.Escape)) // ESCŰ, ������� �ڷΰ��� ��ư ������
        {
            //AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if (setting.activeSelf) // UI�� ����� �ִٸ�
            {
                if (maker.activeSelf) // �����̵� ����� ����
                {
                    settingUI.SetActive(true);
                    maker.SetActive(false);
                    ketChapGaugeBackGround.SetActive(true);
                }
                else
                {
                    setting.SetActive(false);
                    BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
                }

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
        setting.SetActive(!setting.activeSelf);
        BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
        Time.timeScale = setting.activeSelf == true ? 0f : 1f;
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
        setting.SetActive(false);
    }
}
