namespace MainApplication.Localization;

public abstract class LanguageCheck
{
    public static bool CorrectLanguage(string language)
    {
        return language switch
        {
            "en-US" => true,
            "fr-FR" => true,
            _ => false,
        };
    }
}
// Mettre en place la gestion auto des langues --> si langue clavier = Fr alors appli Fr sinon EN
// Ici le code en exemple 
//public System.Globalization.CultureInfo Culture { get; }
//public void MyCulture() {
   // Gets the current input language.
   //InputLanguage myCurrentLanguage = InputLanguage.CurrentInputLanguage;

   // Gets the culture for the language  and prints it.
  // CultureInfo myCultureInfo = myCurrentLanguage.Culture;
  // textBox1.Text = myCultureInfo.EnglishName;
//}
