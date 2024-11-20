using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;
    public Slider slider;
    public Button reStartButton;

    private float preVolume; // ���� ���� ũ��
    private bool sound = false; // false: �Ҹ� �� // true: �Ҹ� Ŵ

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Sound"))
            sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
        else
            PlayerPrefs.SetInt("Sound", 1);

        slider.value = PlayerPrefs.GetFloat("Value");
        PlayerPrefs.SetFloat("preValue", 0.1f);
    }

    private void Update()
    {
        AudioManager.Instance.SetBGMVolume(slider.value);

        if(slider.value > 0)
            AudioManager.Instance.ResumeBGM();

        SliderUpdate();

        sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
    }


    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetSoundSetting(sound);
    }

    //private void SetGameVersion()
    //{
    //    gameVersionTxt.text = $"Version:{Application.version}";
    //}

    private void SetSoundSetting(bool sound) // On/Off ��ư Ȱ��ȭ, ��Ȱ��ȭ
    {
        soundOnToggle.SetActive(sound);
        soundOffToggle.SetActive(!sound);
    }

    public void OnClickSoundOnToggle() // ���� ���� �־ ������
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // ���߿� sound Ű�� slider �� ������ ���� �ӽ� ����
        PlayerPrefs.SetFloat("preValue", slider.value);

        PlayerPrefs.SetInt("Sound", 0);
        PlayerPrefs.SetFloat("Value", 0.0f);
        SetSoundSetting(sound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.PauseBGM();

        slider.value = 0f; 
    }

    public void OnClickSoundOffToggle() // ���� ���� �־ ų����
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("Sound", 1);
        PlayerPrefs.SetFloat("Value", preVolume);
        SetSoundSetting(sound);

        AudioManager.Instance.ResumeBGM();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        slider.value = PlayerPrefs.GetFloat("preValue"); // ������ slider �� ����
    }


    public void OnClickSettingQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf == true ? 0f : 1f;
    }

    public void ReStartBtn()
    {
        reStartButton.interactable = false;
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        SceneLoader.Instance.ReloadScene();
    }

    private void SliderUpdate() 
    {
        // �����̴� 0���� �����ϸ� ���� ����� OFF�� �ٲ�
        // �����̴� 0 �ʰ��� �����ϸ� ���� ����� OnF�� �ٲ�
        soundOnToggle.SetActive(slider.value > 0.0f);
        soundOffToggle.SetActive(slider.value == 0.0f);
        PlayerPrefs.SetFloat("Value", slider.value);
    }
}
