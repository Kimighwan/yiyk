using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public GameObject[] tutEn;
    public GameObject[] tutKo;

    private void Update()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) // 영어
        {
            foreach (GameObject go in tutEn)
            {
                go.SetActive(true);
            }

            foreach (GameObject go in tutKo)
            {
                go.SetActive(false);
            }
        }
        else    // 한글
        {
            foreach (GameObject go in tutEn)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in tutKo)
            {
                go.SetActive(true);
            }
        }
    }
}
