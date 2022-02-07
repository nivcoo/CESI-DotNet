using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using MainApplication.ViewModels;
using Windows.Storage;
using System.Threading.Tasks;
using MainApplication.Objects;
using MainApplication.Objects.Enums;

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
        if (saveName == null || SourcePath == null || TargetPath == null || saveType == null)
        {
            ErrorTextBlock.Text = "You have to complete all input !";
            args.Cancel = true;
            return;
        }

        if(_createSaveViewModel.AlreadySaveWithSameName(saveName) != null)
        {
            ErrorTextBlock.Text = "This save already exist, please select another name !";
            args.Cancel = true;
            return;
        }

        TypeSave saveTypeEnum = (TypeSave) saveType;

        Save save = new(saveName, SourcePath, TargetPath, saveTypeEnum, State.End, 0, 0, 0, 0);
        
        _createSaveViewModel.AddNewSave(save);

    }
}

