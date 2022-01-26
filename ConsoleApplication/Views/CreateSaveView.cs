using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.Services;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class CreateSaveView
{
    private readonly CreateSaveViewModel _easySaveViewModel;

    public CreateSaveView()
    {
        _easySaveViewModel = new CreateSaveViewModel();
        InitView();
    }

    private void InitView()
    {
        SetName();
        SetSourceUri();
        SetTargetUri();
        SetTypeSave();
        SetState();
    }

    private void SetState()
    {
        _easySaveViewModel.Save.State = State.Active;
    }

    private void SetTypeSave()
    {
        Console.Write("\nDonner type de sauvegarde ( 1 - Complete | 2 - Differential ) : ");
        var saveTypeSave = Console.ReadLine();
        while (!SaveService.GetInstance().IsValidTypeSave(saveTypeSave))
        {
            Console.Write("\nChoisissez 1 ou 2 pour les types de sauvegarde ( 1 - Complete | 2 - Differential ) : ");
            saveTypeSave = Console.ReadLine();
        }
        _easySaveViewModel.Save.Type = SaveService.GetInstance().StringToTypeSave(saveTypeSave);
    }
    
    private void SetName()
    {
        Console.Write("\nDonner un nom à la sauvegarde : ");
        var saveName = Console.ReadLine();
        while (!SaveService.GetInstance().AlreadySaveWithSameName(saveName))
        {
            Console.Write("\nUne sauvegarde comporte déjà ce nom. Donner un nom à la sauvegarde : ");
            saveName = Console.ReadLine();
        }

        _easySaveViewModel.Save.Name = saveName;
    }

    private void SetSourceUri()
    {
        Console.Write("\nDonner le chemin d'accès à la sauvegarde : ");
        var sourceUri = Console.ReadLine();
        while (!SaveService.GetInstance().IsValidUri(sourceUri))
        {
            Console.Write("\nLe chemin d'accès est incorrect. Donner un chemin d'accès : ");
            sourceUri = Console.ReadLine();
        }

        _easySaveViewModel.Save.SourcePath = SaveService.GetInstance().StringToUri(sourceUri);
    }

    private void SetTargetUri()
    {
        Console.Write("\nDonner le chemin d'écriture de la sauvegarde : ");
        var targetUri = Console.ReadLine();
        while (!SaveService.GetInstance().IsValidUri(targetUri))
        {
            Console.Write("\nLe chemin est incorrect. Donner un chemin d'écriture : ");
            targetUri = Console.ReadLine();
        }

        _easySaveViewModel.Save.TargetPath = SaveService.GetInstance().StringToUri(targetUri);
    }
}