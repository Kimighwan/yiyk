using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;
    public Slider slider;
   
    private float preValue; // 직전 볼륨 크기
    private bool sound = true; // false: 소리 끔 // true: 소리 킴


    private void Update()
    {
        AudioManager.Instance.SetBGMVolume(slider.value);
    }


    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        // SetGameVersion();
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

        preValue = slider.value; // 나중에 sound 키면 slider 값 복구를 위해 임시 저장
        sound = false;

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.Mute();

        SetSoundSetting(sound);
        slider.value = 0f; 
    }

    public void OnClickSoundOffToggle() // 현재 꺼져 있어서 킬꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        sound = true;

        AudioManager.Instance.UnMute();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        SetSoundSetting(sound);
        slider.value = preValue; // 끄기전 slider 값 복구
    }


    public void OnClickSettingQuit()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = gameObject.activeSelf == true ? 0f : 1f;
    }

    public void ReStartBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        SceneLoader.Instance.ReloadScene();
    }
}
