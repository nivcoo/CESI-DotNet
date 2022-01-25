using System.Text;
using System.Xml.Schema;
using MainApplication.Localization;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class EasySaveView
{
    private readonly EasySaveViewModel _easySaveViewModel;

    public EasySaveView()
    {
        _easySaveViewModel = new EasySaveViewModel();
        InitView();
    }

    private void InitView()
    {
        Console.WriteLine("Bienvenue dans EasySave");
        SelectLanguage();
        ClientManager();
    }

    private void ClientManager()
    {
        var motd = new StringBuilder();
        motd.AppendLine("Sélectionnez votre choix : ");
        motd.AppendLine("");
        motd.AppendLine((int) Choice.ShowList + " : Afficher la liste des sauvegardes ");
        motd.AppendLine((int) Choice.Create + " : Ajouter une sauvegarde ");
        motd.AppendLine((int) Choice.Remove + " : Supprimer une sauvegarde");
        motd.AppendLine((int) Choice.Start + " : Lancer une ou plusieurs sauvegardes");
        motd.AppendLine((int) Choice.Stop + " : Arrêter une ou plusieurs sauvegardes");
        Console.Write(motd);


        Choice? choice = null;
        while (choice == null)
        {
            Console.Write("\nVotre choix : ");
            var choiceString = Console.ReadLine();

            try
            {
                if (choiceString != null)
                {
                    var convertedChoice = Convert.ToInt32(choiceString);
                    if (Enum.IsDefined(typeof(Choice), convertedChoice))
                        choice = (Choice) convertedChoice;
                }
            }
            catch (FormatException)
            {
            }
        }

        ShowSaves();
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

    private void ShowSaves()
    {
        _easySaveViewModel.UpdateSaves();
        var saves = _easySaveViewModel.Saves;

        List<Tuple<string?, string?, string?, string?, string?>> tuplesSaves = new();

        if (saves != null)
            foreach (var save in saves)
            {
                tuplesSaves.Add(Tuple.Create(save.Name, save.State?.ToString(), save.Type?.ToString(),
                    save.SourcePath?.ToString(), save.TargetPath?.ToString()));
            }

        Console.WriteLine(tuplesSaves.ToStringTable(
            new[] {"Nom", "Etat", "Type", "Source", "Destination"},
            a => a.Item1 ?? string.Empty, a => a.Item2 ?? string.Empty,
            a => a.Item3 ?? string.Empty
            , a => a.Item4 ?? string.Empty, a => a.Item5 ?? string.Empty));


        Console.WriteLine();
    }
}