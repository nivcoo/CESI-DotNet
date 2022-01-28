using MainApplication.Localization;

namespace ConsoleApplication.Views;

/// <summary>
/// The base and generic viewmodel for the application 
/// </summary>
/// 
public class BaseView
{
    protected static void AskReturnMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine(Language.GLOBAL_ASK_GO_TO_MENU);
        Console.ReadLine();
    }
}