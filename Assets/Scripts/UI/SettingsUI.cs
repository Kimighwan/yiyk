using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;
    public Slider slider;
    public Button reStartButton;

    private float preVolume; // 직전 사운드 크기
    private bool sound = false; // false: 소리 끔 // true: 소리 킴

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

    private void SetSoundSetting(bool sound) // On/Off 버튼 활성화, 비활성화
    {
        soundOnToggle.SetActive(sound);
        soundOffToggle.SetActive(!sound);
    }

    public void OnClickSoundOnToggle() // 현재 켜져 있어서 끌꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // 나중에 sound 키면 slider 값 복구를 위해 임시 저장
        PlayerPrefs.SetFloat("preValue", slider.value);

        PlayerPrefs.SetInt("Sound", 0);
        PlayerPrefs.SetFloat("Value", 0.0f);
        SetSoundSetting(sound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.PauseBGM();

        slider.value = 0f; 
    }

    public void OnClickSoundOffToggle() // 현재 꺼져 있어서 킬꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("Sound", 1);
        PlayerPrefs.SetFloat("Value", preVolume);
        SetSoundSetting(sound);

        AudioManager.Instance.ResumeBGM();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        slider.value = PlayerPrefs.GetFloat("preValue"); // 끄기전 slider 값 복구
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
        // 슬라이더 0으로 조절하면 사운드 토글이 OFF로 바뀜
        // 슬라이더 0 초과로 조절하면 사운드 토글이 OnF로 바뀜
        soundOnToggle.SetActive(slider.value > 0.0f);
        soundOffToggle.SetActive(slider.value == 0.0f);
        PlayerPrefs.SetFloat("Value", slider.value);
    }
}
