namespace MainApplication.Localization;

/// <summary>
///     Get the language selected by the user
/// </summary>
public static class LanguageCheck
{
    public static readonly string[] Languages = {"en-US", "fr-FR"};

    public static bool CorrectLanguage(string language)
    {
        return Languages.Contains(language);
    }
}