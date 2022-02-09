namespace MainApplication.Localization;
/// <summary>
/// Here is the language check function
/// </summary>
public static class LanguageCheck
{

    public static readonly string[] Languages = { "en-US", "fr-FR" };
    public static bool CorrectLanguage(string language)
    {
        return Languages.Contains(language);
    }
}
