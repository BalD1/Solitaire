using System;

public static class LocalizationManagerEventsHandler
{
    public static event Action<LocalizationManager.Languages> OnLanguageChanged;
    public static void LanguageChanged(this LocalizationManager localizationManager, LocalizationManager.Languages newLanguage)
        => OnLanguageChanged?.Invoke(newLanguage);
}
