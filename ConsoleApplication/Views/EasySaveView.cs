using System.Text;
using System.Xml.Schema;
using ConsoleApplication.Libs;
using MainApplication.Localization;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class EasySaveView
{
    private readonly EasySaveViewModel _easySaveViewModel;
    private bool running;

    public EasySaveView()
    {
        _easySaveViewModel = new EasySaveViewModel();
        running = true;
        InitView();
    }

    private void InitView()
    {
        Console.WriteLine("Bienvenue dans EasySave");
        SelectLanguage();
        SelectActions();
    }

    private void SelectActions()
    {
        while (running)
        {
            var motd = new StringBuilder();
            motd.AppendLine("Sélectionnez votre choix : ");
            motd.AppendLine("");
            motd.AppendLine((int) Choice.ShowList + " : Afficher la liste des sauvegardes ");
            motd.AppendLine((int) Choice.Create + " : Ajouter une sauvegarde ");
            motd.AppendLine((int) Choice.Remove + " : Supprimer une sauvegarde");
            motd.AppendLine((int) Choice.Start + " : Lancer une ou plusieurs sauvegardes");
            motd.AppendLine((int) Choice.Stop + " : Arrêter une ou plusieurs sauvegardes");
            motd.AppendLine((int) Choice.End + " : Stopper l'application");
            Console.Write(motd);
            ChoiceSelector();
        }
    }

    private void ChoiceSelector()
    {
        Console.Write("\nVotre choix : ");
        var choice = BaseViewModel.ConvertStringIntegerToEnum<Choice>(Console.ReadLine());
        while (choice == default)
        {
            Console.Write("\nChoix incorrect. Votre choix : ");
            choice = BaseViewModel.ConvertStringIntegerToEnum<Choice>(Console.ReadLine());
        }

        switch (choice)
        {
            case Choice.Create:
                var createSaveView = new CreateSaveView();
                createSaveView.InitView();
                break;
            case Choice.ShowList:
                ShowSavesList();
                break;
            case Choice.Remove:
                break;
            case Choice.Start:
                break;
            case Choice.Stop:
                break;
            case Choice.End:
                StopApplication();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SelectSave()
    {
    }


    private void SelectLanguage()
    {
        var success = false;
        while (!success)
        {
            Console.WriteLine("Please Select Language : en-US, fr-FR");
            _easySaveViewModel.LanguageString = Console.ReadLine();
            success = _easySaveViewModel.UpdateLanguage();
        }

        Console.WriteLine(Language.GLOBAL_SELECTED_LANGUAGE);
    }

    private void ShowSavesList()
    {
        _easySaveViewModel.UpdateSavesList();
        var saves = _easySaveViewModel.Saves;

        List<Tuple<string, string, string, string, string>> tuplesSaves = new();

        if (saves != null)
            tuplesSaves.AddRange(saves.Select(save => Tuple.Create(save.Name, save.State.ToString(),
                save.Type.ToString(), save.SourcePath.ToString(), save.TargetPath.ToString())));

        Console.WriteLine(
            tuplesSaves.ToStringTable(new[] {"Nom", "Etat", "Type", "Source", "Destination"},
                a => a.Item1,
                a => a.Item2,
                a => a.Item3,
                a => a.Item4,
                a => a.Item5
            )
        );


        Console.WriteLine();
    }

    private void StopApplication()
    {
        running = false;
    }
}