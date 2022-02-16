using GuiApplication.Views.Dialogs;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Globalization;

namespace GuiApplication.Views.Pages;

public sealed partial class HomePage : Page
{

    private readonly HomeViewModel _homeViewModel = new();


    public HomePage()
    {
        DataContext = _homeViewModel;
        InitializeComponent();
        InitTexts();
        LanguageComboBox.SelectedItem = _homeViewModel.SelectedCultureInfo;
        LanguageComboBox.SelectionChanged += ChangeCultureEvent;

        SaveFileTypeComboBox.SelectedItem = _homeViewModel.SelectedSaveFileType;
        SaveFileTypeComboBox.SelectionChanged += ChangeSaveFileTypeEvent;
    }

    private void InitTexts()
    {
        WelcomeTextBox.Text = MainApplication.Localization.Language.GLOBAL_WELCOME;
        SelectLanguageTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_LANGUAGE_GUI;
        SelectSaveFileTypeTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_SAVE_FILE_TYPE_GUI;

        EncryptExtensionsTextBox.Text = MainApplication.Localization.Language.GLOBAL_ENCRYPT_EXTENSIONS_GUI;

        PriorityFilesTextBox.Text = MainApplication.Localization.Language.GLOBAL_PRIORITY_FILES_GUI;

        ButtonAddExtensionTextBlock.Content = MainApplication.Localization.Language.HOME_ADD_EXTENSION;

        ButtonAddFileTextBlock.Content = MainApplication.Localization.Language.HOME_ADD_PRIORITY_FILE;
    }

    private void ChangeCultureEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var language = comboBox.SelectedItem as CultureInfo;
        _homeViewModel.SelectedCultureInfo = language;

        MainWindow.GetInstance().InitTexts();

        InitTexts();
    }

    private void ChangeSaveFileTypeEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var saveFileType = (SaveFileType)comboBox.SelectedItem;
        _homeViewModel.SelectedSaveFileType = saveFileType;
    }

    private async void AddEncryptExtension_OpenDialog(object sender, RoutedEventArgs e)
    {
        var content = new AddEncryptExtensionDialog();
        ContentDialog dialog = new()
        {
            Title = MainApplication.Localization.Language.BUTTON_CREATE_EXTENSION,
            PrimaryButtonText = MainApplication.Localization.Language.GLOBAL_CREATE,
            CloseButtonText = MainApplication.Localization.Language.GLOBAL_CANCEL,
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };
        dialog.XamlRoot = this.Content.XamlRoot;
        dialog.PrimaryButtonClick += content.CreateButtonEvent;
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            _homeViewModel.UpdateEncryptExtensionsList();

    }


    private async void AddPriorityFile_OpenDialog(object sender, RoutedEventArgs e)
    {
        var content = new AddPriorityFileDialog();
        ContentDialog dialog = new()
        {
            Title = MainApplication.Localization.Language.BUTTON_CREATE_PRIORITY_FILE,
            PrimaryButtonText = MainApplication.Localization.Language.GLOBAL_CREATE,
            CloseButtonText = MainApplication.Localization.Language.GLOBAL_CANCEL,
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };
        dialog.XamlRoot = this.Content.XamlRoot;
        dialog.PrimaryButtonClick += content.CreateButtonEvent;
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            _homeViewModel.UpdatePriorityFilesList();

    }
}



