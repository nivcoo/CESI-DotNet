using MainApplication.Localization;

namespace ConsoleApplication.Views;

public class BaseView
{
    protected static void AskReturnMainMenu()
    {
        Console.WriteLine(Language.GLOBAL_ASK_GO_TO_MENU);
        Console.ReadLine();
    }
}