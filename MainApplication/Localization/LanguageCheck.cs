namespace MainApplication.Localization;

/// <summary>
/// Get the language selected by the user 
/// </summary>
public abstract class LanguageCheck
{
    public static bool CorrectLanguage(string language)
    {
        return language switch
        {
            "en-US" => true,
            "fr-FR" => true,
            _ => false
        };
    }
}
