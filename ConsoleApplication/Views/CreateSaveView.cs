using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;
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

        Console.WriteLine(CreateSave()
            ? "Votre sauvegarder a été créée avec succès !"
            : "Une erreur est survenue pendant la création de votre sauvegarde !");
    }
    
    private void AskSaveName()
    {
        Console.Write("\nDonner un nom à la sauvegarde : ");
        var saveName = Console.ReadLine();
        while (saveName == null || _createSaveViewModel.AlreadySaveWithSameName(saveName))
        {
            Console.Write("\nUne sauvegarde comporte déjà ce nom. Donner un nom à la sauvegarde : ");
            saveName = Console.ReadLine();
        }

        Name = saveName;
    }
    private void AskSaveSourceUri()
    {
        Console.Write("\nDonner le chemin d'accès à la sauvegarde : ");
        Uri? uri = null;
        uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        while (uri == null)
        {
            Console.Write("\nLe chemin d'accès est incorrect. Donner un chemin d'accès : ");
            uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        }

        SourcePath = uri;
    }

    private void AskSaveTargetUri()
    {
        Console.Write("\nDonner le chemin d'écriture de la sauvegarde : ");
        Uri? uri = null;
        uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        while (uri == null)
        {
            Console.Write("\nLe chemin est incorrect. Donner un chemin d'écriture : ");
            uri = CreateSaveViewModel.IsValidUri(Console.ReadLine());
        }

        TargetPath = uri;
    }
    
    private void AskSaveTypeSave()
    {
        
        Console.Write("\nDonner type de sauvegarde ( 1 - Complete | 2 - Differential ) : ");
        var typeSave = BaseViewModel.ConvertStringIntegerToEnum<TypeSave>(Console.ReadLine());
        while (typeSave == default)
        {
            Console.Write("\nChoisissez 1 ou 2 pour les types de sauvegarde ( 1 - Complete | 2 - Differential ) : ");
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