using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    // public TextMeshProUGUI gameVersionTxt;

    public GameObject soundOnToggle;
    public GameObject soundOffToggle;
    public Slider slider;
   
    private float preValue; // ���� ���� ũ��
    private bool sound = true; // false: �Ҹ� �� // true: �Ҹ� Ŵ


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

    private void SetSoundSetting(bool sound) // On/Off ��ư Ȱ��ȭ, ��Ȱ��ȭ
    {
        soundOnToggle.SetActive(sound);
        soundOffToggle.SetActive(!sound);
    }

    public void OnClickSoundOnToggle() // ���� ���� �־ ������
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        preValue = slider.value; // ���߿� sound Ű�� slider �� ������ ���� �ӽ� ����
        sound = false;

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.Mute();

        SetSoundSetting(sound);
        slider.value = 0f; 
    }

    public void OnClickSoundOffToggle() // ���� ���� �־ ų����
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        sound = true;

        AudioManager.Instance.UnMute();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        SetSoundSetting(sound);
        slider.value = preValue; // ������ slider �� ����
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
