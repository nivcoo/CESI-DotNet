namespace MainApplication.Localization;
/// <summary>
/// Here is the language check function
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
