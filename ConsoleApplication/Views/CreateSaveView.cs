using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;
/// <summary>
/// Here we can create a view
/// </summary>
public class CreateSaveView : BaseView
{
    private readonly CreateSaveViewModel _createSaveViewModel;

    private bool _cancel;

    private string? Name { get; set; }
    private Uri? SourcePath { get; set; }
    private Uri? TargetPath { get; set; }
    private TypeSave Type { get; set; }

    public CreateSaveView()
    {
        _createSaveViewModel = new CreateSaveViewModel();
    }

    public void InitView() 
    {
        AskSaveName(); 
        AskSaveSourceUri();
        AskSaveTargetUri();
        AskSaveTypeSave();

        if (!_cancel)
            Console.WriteLine(CreateSave() ? Language.CREATE_SAVE_SUCCESS : Language.CREATE_SAVE_ERROR);

        AskReturnMainMenu();
    }

    private void AskSaveName() 
        /// <summary>
        /// Get the name for the save
        /// </summary>
    {
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_NAME + @" ");
        var saveName = Console.ReadLine();
        while (saveName == null || _createSaveViewModel.AlreadySaveWithSameName(saveName) != null)
        {
            Console.WriteLine(Language.CREATE_SAVE_ASK_NAME_RETRY);
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_NAME + @" ");
            saveName = Console.ReadLine();
        }

        if (saveName == "cancel")
            _cancel = true;
        else
            Name = saveName;
    }

    private void AskSaveSourceUri()
    
        /// <summary>
        ///Get the source file's URI
        /// </summary>
    {
        if (_cancel)
            return;
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_SOURCE_URI + @" ");
        var consoleRead = Console.ReadLine();
        var uri = CreateSaveViewModel.IsValidUri(consoleRead);

        while (consoleRead != "cancel" && uri == null)
        {
            Console.WriteLine(Language.CREATE_SAVE_ASK_SOURCE_URI_RETRY);
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_SOURCE_URI + @" ");
            consoleRead = Console.ReadLine();
            uri = CreateSaveViewModel.IsValidUri(consoleRead);
        }

        if (uri == null)
            _cancel = true;
        else
            SourcePath = uri;
    }

    private void AskSaveTargetUri()
    
        /// <summary>
        /// Get target's location URI
        /// </summary>
    {
        if (_cancel)
            return;
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TARGET_URI + @" ");
        var consoleRead = Console.ReadLine();
        var uri = CreateSaveViewModel.IsValidUri(consoleRead);
        while (consoleRead != "cancel" && uri == null)
        {
            Console.WriteLine(Language.CREATE_SAVE_ASK_TARGET_URI_RETRY);
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TARGET_URI + @" ");
            consoleRead = Console.ReadLine();
            uri = CreateSaveViewModel.IsValidUri(consoleRead);
        }

        if (uri == null)
            _cancel = true;
        else
            TargetPath = uri;
    }

    private void AskSaveTypeSave()
    
        /// <summary>
        /// get the save's type : complete / differential
        /// </summary>
    {
        if (_cancel)
            return;
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TYPE + @" ");
        var consoleRead = Console.ReadLine();
        var typeSave = BaseViewModel.ConvertStringIntegerToEnum<TypeSave>(consoleRead);
        while (consoleRead != "cancel" && typeSave == default)
        {
            Console.WriteLine(Language.CREATE_SAVE_ASK_TYPE_RETRY);
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TYPE + @" ");
            consoleRead = Console.ReadLine();
            typeSave = BaseViewModel.ConvertStringIntegerToEnum<TypeSave>(consoleRead);
        }

        if (typeSave == default)
            _cancel = true;
        else
            Type = typeSave;
    }

    private bool CreateSave()
        /// <summary>
        /// Create a new save profile will all the informations provided by the user
        /// </summary>
    {
        if (Name != null && SourcePath != null && TargetPath != null)
            return _createSaveViewModel.AddNewSave(new Save(Name, SourcePath, TargetPath, Type, State.End,
                0, 0, 0, 0));
        return false;
    }
}