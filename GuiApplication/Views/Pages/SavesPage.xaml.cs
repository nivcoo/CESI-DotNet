using GuiApplication.Views.Dialogs;
using MainApplication.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GuiApplication.Views.Pages;

public sealed partial class SavesPage : Page
{

    private readonly SavesViewModel _saveViewModel = new();

    public SavesPage()
    {
        DataContext = _saveViewModel;

        if (_saveViewModel.DispatchUiAction == null)
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _saveViewModel.DispatchUiAction = (action) => dispatcherQueue.TryEnqueue(() => { action.Invoke(); });
        }
        InitializeComponent();
    }

    private async void CreateSave_OpenDialog(object sender, RoutedEventArgs e)
    {
        var content = new CreateSaveDialog();
        ContentDialog dialog = new()
        {
            Title = "Create new Save",
            PrimaryButtonText = "Create",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };
        dialog.XamlRoot = this.Content.XamlRoot;
        dialog.PrimaryButtonClick += content.CreateButtonEvent;
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            _saveViewModel.UpdateSavesList();

    }
}

