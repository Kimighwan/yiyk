using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundBGMOnToggle;
    public GameObject soundBGMOffToggle;

    public GameObject soundSFXOnToggle;
    public GameObject soundSFXOffToggle;

    public Slider BGMslider;
    public Slider SFXslider;

    public Button reStartButton;

    public GameObject BackGroundFadeImg;
    public GameObject makerImg;
    public GameObject ketChapGaugeBackGround;

    private float preBGMVolume; // ���� ���� ũ��
    private bool bgmSound = false; // false: �Ҹ� �� // true: �Ҹ� Ŵ

    private float preSFXVolume; // ���� ���� ũ��
    private bool sfxSound = false; // false: �Ҹ� �� // true: �Ҹ� Ŵ

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // BGM
        if (PlayerPrefs.HasKey("BGMSound"))
            bgmSound = PlayerPrefs.GetInt("BGMSound") == 1 ? true : false;
        else
            PlayerPrefs.SetInt("BGMSound", 1);

        // SFX
        if (PlayerPrefs.HasKey("SFXSound"))
            sfxSound = PlayerPrefs.GetInt("SFXSound") == 1 ? true : false;
        else
            PlayerPrefs.SetInt("SFXSound", 1);

        BGMslider.value = PlayerPrefs.GetFloat("BGMValue");
        SFXslider.value = PlayerPrefs.GetFloat("SFXValue");
        PlayerPrefs.SetFloat("preBGMVolume", 0.2f);
        PlayerPrefs.SetFloat("preSFXVolume", 0.5f);
    }

    private void Update()
    {
        AudioManager.Instance.SetBGMVolume(BGMslider.value);
        AudioManager.Instance.SetSFXVolume(SFXslider.value);

        if(BGMslider.value > 0)
            AudioManager.Instance.ResumeBGM();

        SliderUpdate();

        bgmSound = PlayerPrefs.GetInt("BGMSound") == 1 ? true : false;
        sfxSound = PlayerPrefs.GetInt("SFXSound") == 1 ? true : false;
    }


    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetBGMSoundSetting(bgmSound);
        SetSFXSoundSetting(sfxSound);
    }

    //private void SetGameVersion()
    //{
    //    gameVersionTxt.text = $"Version:{Application.version}";
    //}

    private void SetBGMSoundSetting(bool sound) // On/Off ��ư Ȱ��ȭ, ��Ȱ��ȭ
    {
        soundBGMOnToggle.SetActive(sound);
        soundBGMOffToggle.SetActive(!sound);
    }

    private void SetSFXSoundSetting(bool sound) // On/Off ��ư Ȱ��ȭ, ��Ȱ��ȭ
    {
        soundSFXOnToggle.SetActive(sound);
        soundSFXOffToggle.SetActive(!sound);
    }

    public void OnClickBGMSoundOnToggle() // ���� ���� �־ ������
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // ���߿� sound Ű�� slider �� ������ ���� �ӽ� ����
        PlayerPrefs.SetFloat("preBGMVolume", BGMslider.value);

        PlayerPrefs.SetInt("BGMSound", 0);
        PlayerPrefs.SetFloat("BGMValue", 0.0f);
        SetBGMSoundSetting(bgmSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.PauseBGM();

        BGMslider.value = 0f; 
    }

    public void OnClickSFXSoundOnToggle() // ���� ���� �־ ������
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // ���߿� sound Ű�� slider �� ������ ���� �ӽ� ����
        PlayerPrefs.SetFloat("preSFXVolume", SFXslider.value);

        PlayerPrefs.SetInt("SFXSound", 0);
        PlayerPrefs.SetFloat("SFXValue", 0.0f);
        SetSFXSoundSetting(sfxSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        // AudioManager.Instance.PauseBGM();

        SFXslider.value = 0f;
    }

    public void OnClickBGMSoundOffToggle() // ���� ���� �־ ų����
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("BGMSound", 1);
        PlayerPrefs.SetFloat("BGMValue", preBGMVolume);
        SetBGMSoundSetting(bgmSound);

        AudioManager.Instance.ResumeBGM();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        BGMslider.value = PlayerPrefs.GetFloat("preBGMVolume"); // ������ slider �� ����
    }

    public void OnClickSFXSoundOffToggle() // ���� ���� �־ ų����
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("SFXSound", 1);
        PlayerPrefs.SetFloat("SFXValue", preSFXVolume);
        SetSFXSoundSetting(sfxSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        SFXslider.value = PlayerPrefs.GetFloat("preSFXVolume"); // ������ slider �� ����
    }

    public void OnClickSettingQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        gameObject.SetActive(!gameObject.activeSelf);
        BackGroundFadeImg.SetActive(!BackGroundFadeImg.activeSelf);
        Time.timeScale = gameObject.activeSelf == true ? 0f : 1f;
    }

    public void ReStartBtn()
    {
        reStartButton.interactable = false;
        OnClickSettingQuit();
        SceneLoader.Instance.ReloadScene();
    }

    private void SliderUpdate() 
    {
        // �����̴� 0���� �����ϸ� ���� ����� OFF�� �ٲ�
        // �����̴� 0 �ʰ��� �����ϸ� ���� ����� OnF�� �ٲ�
        soundBGMOnToggle.SetActive(BGMslider.value > 0.0f);
        soundBGMOffToggle.SetActive(BGMslider.value == 0.0f);
        PlayerPrefs.SetFloat("BGMValue", BGMslider.value);

        soundSFXOnToggle.SetActive(SFXslider.value > 0.0f);
        soundSFXOffToggle.SetActive(SFXslider.value == 0.0f);
        PlayerPrefs.SetFloat("SFXValue", SFXslider.value);
    }

    // URL ��ũ ����
    public void OnClickOpenURL(string url)
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        Application.OpenURL(url);
    }

    public void OnClickMaker()
    {
        makerImg.SetActive(true);
        ketChapGaugeBackGround.SetActive(false);
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
    }
    public void OnClickMakerQuit()
    {
        makerImg.SetActive(false);
        ketChapGaugeBackGround.SetActive(true);
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
    }
}
