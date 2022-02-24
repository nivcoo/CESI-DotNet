using GuiApplication.Views.UiWindows;
using MainApplication.Objects;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace GuiApplication.Views.Dialogs;

public sealed partial class CreateSaveDialog : Page
{

    private readonly CreateSaveViewModel _createSaveViewModel = new();

    private Uri SourcePath { get; set; }
    private Uri TargetPath { get; set; }

    public CreateSaveDialog()
    {
        InitializeComponent();
        DataContext = _createSaveViewModel;
        InitTexts();
    }

    private void InitTexts()
    {
        SaveName.PlaceholderText = MainApplication.Localization.Language.GLOBAL_NAME;
        SourcePathButton.Content = MainApplication.Localization.Language.CREATE_SAVE_SELECT_SOURCE;
        TargetPathButton.Content = MainApplication.Localization.Language.CREATE_SAVE_SELECT_TARGET;
    }

    private async void SelectSourcePathAsync(object sender, RoutedEventArgs e)
    {
        var folder = await GetFolderFromFolderPicker();
        if (folder == null)
            return;
        var folderName = folder.Path;
        SourcePath = new Uri(folderName);
        SourcePathText.Text = folderName;
    }

    private async void SelectTargetPathAsync(object sender, RoutedEventArgs e)
    {

        var folder = await GetFolderFromFolderPicker();
        if (folder == null)
            return;
        var folderName = folder.Path + @"\EasySave";
        TargetPath = new Uri(folderName);
        TargetPathText.Text = folderName;
    }


    private static async Task<StorageFolder> GetFolderFromFolderPicker()
    {

        var mainWindow = MainWindow.GetInstance();
        var folderPicker = new FolderPicker();
        InitializeWithWindow.Initialize(folderPicker, mainWindow.HWnd);
        var folder = await folderPicker.PickSingleFolderAsync();
        return folder;
    }

    public void CreateButtonEvent(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string saveName = SaveName.Text;
        var saveType = SaveType.SelectedItem;
        if (saveName == null || saveName.Length == 0 || SourcePath == null || TargetPath == null || saveType == null)
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.CREATE_COMPLETE_ALL_INPUTS;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }

        if (_createSaveViewModel.AlreadySaveWithSameName(saveName) != null)
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.CREATE_SAVE_ASK_NAME_RETRY;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }

        TypeSave saveTypeEnum = (TypeSave)saveType;

        Save save = new(saveName, SourcePath, TargetPath, saveTypeEnum, State.End, 0, 0, 0, 0);

        _createSaveViewModel.AddNewSave(save);

    }
}

