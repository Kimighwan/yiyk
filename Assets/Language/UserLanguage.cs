using UnityEngine;
using UnityEngine.Localization.Settings;

public class UserLanguage : MonoBehaviour
{
    public void ClickLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
