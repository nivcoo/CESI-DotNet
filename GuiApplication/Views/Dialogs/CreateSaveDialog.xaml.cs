using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace GuiApplication.Views.Dialogs;

public sealed partial class CreateSaveDialog : Page
{
    public CreateSaveDialog()
    {
        InitializeComponent();
    }

    private async void OpenSourceSelectionDialogAsync(object sender, RoutedEventArgs e)
    {
        var mainWindow = MainWindow.GetInstance();
        var folderPicker = new FolderPicker();
        InitializeWithWindow.Initialize(folderPicker, mainWindow.HWnd);
        var folder = await folderPicker.PickSingleFolderAsync();

    }
}

