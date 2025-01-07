using UnityEngine;
using UnityEngine.Localization.Settings;

public class UserLanguage : MonoBehaviour
{
    public GameObject En;   // 0
    public GameObject Ko;   // 1

    public void ClickLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

        if(index == 0)  // En
        {
            Ko.SetActive(true);
            En.SetActive(false);
        }
        else
        {
            Ko.SetActive(false);
            En.SetActive(true);
        }
    }
}
