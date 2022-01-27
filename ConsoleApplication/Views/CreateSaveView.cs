using MainApplication.Localization;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class CreateSaveView
{
    private readonly CreateSaveViewModel _createSaveViewModel;

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

        Console.WriteLine(CreateSave() ? Language.CREATE_SAVE_SUCCESS : Language.CREATE_SAVE_ERROR);
    }

    private void AskSaveName()
    {
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_NAME + @" ");
        var saveName = Console.ReadLine();
        while (saveName == null || _createSaveViewModel.AlreadySaveWithSameName(saveName))
        {
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_NAME_RETRY+ @" ");
            saveName = Console.ReadLine();
        }

        Name = saveName;
    }

    private void AskSaveSourceUri()
    {
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_SOURCE_URI + @" ");
        var uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        while (uri == null)
        {
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_SOURCE_URI_RETRY + @" ");
            uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        }

        SourcePath = uri;
    }

    private void AskSaveTargetUri()
    {
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TARGET_URI + @" ");
        var uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        while (uri == null)
        {
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TARGET_URI_RETRY + @" ");
            uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        }

        TargetPath = uri;
    }

    private void AskSaveTypeSave()
    {
        Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TYPE + @" ");
        var typeSave = BaseViewModel.ConvertStringIntegerToEnum<TypeSave>(Console.ReadLine());
        while (typeSave == default)
        {
            Console.Write(Environment.NewLine + Language.CREATE_SAVE_ASK_TYPE_RETRY + @" ");
            typeSave = BaseViewModel.ConvertStringIntegerToEnum<TypeSave>(Console.ReadLine());
        }

        Type = typeSave;
    }

    private bool CreateSave()
    {
        if (Name != null && SourcePath != null && TargetPath != null)
            return _createSaveViewModel.AddNewSave(new Save(Name, SourcePath, TargetPath, Type, State.Active,
                0, 0, 0, 0));
        return false;
    }
}