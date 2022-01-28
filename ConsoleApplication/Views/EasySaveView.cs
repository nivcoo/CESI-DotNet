using System.Text;
using ConsoleApplication.Libs;
using MainApplication.Localization;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;

namespace ConsoleApplication.Views;

public class EasySaveView : BaseView
{
    private readonly EasySaveViewModel _easySaveViewModel;
    private bool _running;

    public EasySaveView()
    {
        _easySaveViewModel = new EasySaveViewModel();
        _running = true;
        InitView();
    }

    private void InitView()
    {
        Console.WriteLine(Language.GLOBAL_WELCOME);
        SelectLanguage();
        DisplayMotd();
    }

    private void DisplayMotd()
    {
        while (_running)
        {
            var motd = new StringBuilder();
            motd.AppendLine(Language.CHOICE_DISPLAY);
            motd.AppendLine();
            motd.AppendLine((int) Choice.ShowList + " : " + Language.CHOICE_DESC_SHOW_LIST);
            motd.AppendLine((int) Choice.Create + " : " + Language.CHOICE_DESC_CREATE);
            motd.AppendLine((int) Choice.Remove + " : " + Language.CHOICE_DESC_REMOVE);
            motd.AppendLine((int) Choice.Start + " : " + Language.CHOICE_DESC_START);
            motd.AppendLine((int) Choice.Stop + " : " + Language.CHOICE_DESC_STOP);
            motd.AppendLine((int) Choice.End + " : " + Language.CHOICE_DESC_END);
            Console.WriteLine();
            Console.Write(motd);
            ChoiceSelector();
        }
    }

    private void ChoiceSelector()
    {
        Console.Write(Environment.NewLine + Language.GLOBAL_SELECT_CHOICE + @" ");
        var choice = BaseViewModel.ConvertStringIntegerToEnum<Choice>(Console.ReadLine());
        while (choice == default)
        {
            Console.WriteLine(Language.GLOBAL_SELECT_CHOICE_RETRY);
            Console.Write(Environment.NewLine + Language.GLOBAL_SELECT_CHOICE + @" ");
            choice = BaseViewModel.ConvertStringIntegerToEnum<Choice>(Console.ReadLine());
        }

        switch (choice)
        {
            case Choice.ShowList:
                ShowSavesList();
                break;
            case Choice.Create:
                _easySaveViewModel.UpdateSavesList();
                var saves = _easySaveViewModel.Saves;
                if (saves?.Count < 5)
                {
                    var createSaveView = new CreateSaveView();
                    createSaveView.InitView();
                }
                else
                    Console.WriteLine(Language.CREATE_SAVE_REACH_LIMIT);

                break;
            case Choice.Remove:
                RemoveSave();
                break;
            case Choice.Start:
                StartSave();
                break;
            case Choice.Stop:
                StopSave();
                break;
            case Choice.End:
                StopApplication();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RemoveSave()
    {
        Console.Write(Environment.NewLine + Language.GLOBAL_ASK_SAVE_NAME + @" ");
        var saveName = Console.ReadLine();
        var isRunning = _easySaveViewModel.IsRunningSave(saveName);
        while (saveName != "cancel" &&
               (saveName == null || !_easySaveViewModel.RemoveSave(saveName)))
        {
            Console.WriteLine(isRunning
                ? Language.GLOBAL_CANNOT_REMOVE_RUNNING_SAVE
                : Language.GLOBAL_SAVE_NAME_NOT_EXIST);
            Console.Write(Environment.NewLine + Language.GLOBAL_ASK_SAVE_NAME + @" ");
            saveName = Console.ReadLine();
            isRunning = _easySaveViewModel.IsRunningSave(saveName);
        }

        AskReturnMainMenu();
    }

    private void StartSave()
    {
        Console.Write(Environment.NewLine + Language.GLOBAL_ASK_SAVE_NAME_OR_ALL + @" ");
        var saveName = Console.ReadLine();
        var isRunning = _easySaveViewModel.IsRunningSave(saveName);
        while (saveName != "all" && saveName != "cancel" &&
               (saveName == null || !_easySaveViewModel.StartSave(saveName)))
        {
            Console.WriteLine(isRunning ? Language.GLOBAL_SAVE_ALREADY_RUN : Language.GLOBAL_SAVE_NAME_NOT_EXIST);

            Console.Write(Environment.NewLine + Language.GLOBAL_ASK_SAVE_NAME_OR_ALL + @" ");
            saveName = Console.ReadLine();
            isRunning = _easySaveViewModel.IsRunningSave(saveName);
        }

        var startAll = saveName == "all";
        var save = _easySaveViewModel.AlreadySaveWithSameName(saveName);

        if (startAll)
            _easySaveViewModel.StartAllSaves();

        if (saveName != "cancel")
        {
            Console.Write(Language.GLOBAL_PERFORMING_SAVE + @" ");
            using (var progress = new ProgressBar())
            {
                double percent;
                do
                {
                    percent = save == null
                        ? _easySaveViewModel.GetProgressionOfAllSave()
                        : _easySaveViewModel.GetProgressionOfSave(save);
                    var (item1, totalFiles) = save == null
                        ? _easySaveViewModel.GetFilesInformationsOfAllSave()
                        : _easySaveViewModel.GetFilesInformationsOfSave(save);

                    var doneFiles = totalFiles - item1;
                    progress.UpdateFilesStatus(doneFiles, totalFiles);
                    progress.Report(percent > 0 ? percent / 100 : 0);
                    Thread.Sleep(10);
                } while (percent < 100);

                progress.Report(1);
                Thread.Sleep(10);
            }


            Console.WriteLine(Language.GLOBAL_DONE);
        }

        AskReturnMainMenu();
    }

    private void StopSave()
    {
    }


    private void SelectLanguage()
    {
        var success = false;
        while (!success)
        {
            Console.Write(Environment.NewLine + Language.GLOBAL_SELECT_LANGUAGE + @" ");
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
                save.Type.ToString(), save.SourcePath.LocalPath, save.TargetPath.LocalPath)));

        Console.WriteLine(
            tuplesSaves.ToStringTable(
                new[]
                {
                    Language.GLOBAL_NAME, Language.GLOBAL_STATE, Language.GLOBAL_TYPE, Language.GLOBAL_SOURCE,
                    Language.GLOBAL_TARGET
                },
                a => a.Item1,
                a => a.Item2,
                a => a.Item3,
                a => a.Item4,
                a => a.Item5
            )
        );

        AskReturnMainMenu();
    }

    private void StopApplication()
    {
        _running = false;
    }
}