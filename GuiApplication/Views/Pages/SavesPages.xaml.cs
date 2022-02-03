using GuiApplication.Views.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GuiApplication.Views.Pages;

public sealed partial class SavesPage : Page
{
    public SavesPage()
    {
        InitializeComponent();
    }

    private async void CreateSave_OpenDialog(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new()
        {
            Title = "Create new Save",
            PrimaryButtonText = "Create",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = new CreateSaveDialog()
        };
        dialog.XamlRoot = this.Content.XamlRoot;
        var result = await dialog.ShowAsync();
    }
}

