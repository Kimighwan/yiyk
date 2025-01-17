using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIControl : MonoBehaviour
{
    public static AudioUIControl Instance;

    public GameObject soundBGMOnToggle;
    public GameObject soundBGMOffToggle;

    public GameObject soundSFXOnToggle;
    public GameObject soundSFXOffToggle;

    public Slider BGMslider;
    public Slider SFXslider;

    private float preBGMVolume; // 직전 사운드 크기
    private bool bgmSound = false; // false: 소리 끔 // true: 소리 킴

    private float preSFXVolume; // 직전 사운드 크기
    private bool sfxSound = false; // false: 소리 끔 // true: 소리 킴

    private void Awake()
    {
        Instance = this;

        Init();

        SetBGMSoundSetting(bgmSound);
        SetSFXSoundSetting(sfxSound);
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
    }

    private void Update()
    {
        AudioManager.Instance.SetBGMVolume(BGMslider.value);
        AudioManager.Instance.SetSFXVolume(SFXslider.value);

        if (BGMslider.value > 0)
            AudioManager.Instance.ResumeBGM();

        SliderUpdate();

        bgmSound = PlayerPrefs.GetInt("BGMSound") == 1 ? true : false;
        sfxSound = PlayerPrefs.GetInt("SFXSound") == 1 ? true : false;
    }

    private void SetBGMSoundSetting(bool sound) // On/Off 버튼 활성화, 비활성화
    {
        soundBGMOnToggle.SetActive(sound);
        soundBGMOffToggle.SetActive(!sound);
    }

    private void SetSFXSoundSetting(bool sound) // On/Off 버튼 활성화, 비활성화
    {
        soundSFXOnToggle.SetActive(sound);
        soundSFXOffToggle.SetActive(!sound);
    }

    public void OnClickBGMSoundOnToggle() // 현재 켜져 있어서 끌꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // 나중에 sound 키면 slider 값 복구를 위해 임시 저장
        PlayerPrefs.SetFloat("preBGMVolume", BGMslider.value);

        PlayerPrefs.SetInt("BGMSound", 0);
        PlayerPrefs.SetFloat("BGMValue", 0.0f);
        SetBGMSoundSetting(bgmSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        AudioManager.Instance.PauseBGM();

        BGMslider.value = 0f;
    }

    public void OnClickSFXSoundOnToggle() // 현재 켜져 있어서 끌꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        // preVolume = slider.value; // 나중에 sound 키면 slider 값 복구를 위해 임시 저장
        PlayerPrefs.SetFloat("preSFXVolume", SFXslider.value);

        PlayerPrefs.SetInt("SFXSound", 0);
        PlayerPrefs.SetFloat("SFXValue", 0.0f);
        SetSFXSoundSetting(sfxSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);
        // AudioManager.Instance.PauseBGM();

        SFXslider.value = 0f;
    }

    public void OnClickBGMSoundOffToggle() // 현재 꺼져 있어서 킬꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("BGMSound", 1);
        PlayerPrefs.SetFloat("BGMValue", preBGMVolume);
        SetBGMSoundSetting(bgmSound);

        AudioManager.Instance.ResumeBGM();
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        BGMslider.value = PlayerPrefs.GetFloat("preBGMVolume"); // 끄기전 slider 값 복구
    }

    public void OnClickSFXSoundOffToggle() // 현재 꺼져 있어서 킬꺼임
    {
        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        PlayerPrefs.SetInt("SFXSound", 1);
        PlayerPrefs.SetFloat("SFXValue", preSFXVolume);
        SetSFXSoundSetting(sfxSound);

        AudioManager.Instance.PlaySFX(SFX.ButtonClick);

        SFXslider.value = PlayerPrefs.GetFloat("preSFXVolume"); // 끄기전 slider 값 복구
    }

    private void SliderUpdate()
    {
        // 슬라이더 0으로 조절하면 사운드 토글이 OFF로 바뀜
        // 슬라이더 0 초과로 조절하면 사운드 토글이 OnF로 바뀜
        soundBGMOnToggle.SetActive(BGMslider.value > 0.0f);
        soundBGMOffToggle.SetActive(BGMslider.value == 0.0f);
        PlayerPrefs.SetFloat("BGMValue", BGMslider.value);

        soundSFXOnToggle.SetActive(SFXslider.value > 0.0f);
        soundSFXOffToggle.SetActive(SFXslider.value == 0.0f);
        PlayerPrefs.SetFloat("SFXValue", SFXslider.value);
    }
}
