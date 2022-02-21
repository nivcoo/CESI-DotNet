using GuiApplication.Views.Dialogs;
using MainApplication.Objects.Enums;
using MainApplication.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
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

        SavesFileTypeComboBox.SelectedItem = _homeViewModel.SelectedSavesFileType;
        SavesFileTypeComboBox.SelectionChanged += ChangeSavesFileTypeEvent;

        LogsFileTypeComboBox.SelectedItem = _homeViewModel.SelectedLogsFileType;
        LogsFileTypeComboBox.SelectionChanged += ChangeLogsFileTypeEvent;

    }

    private void InitTexts()
    {
        WelcomeTextBox.Text = MainApplication.Localization.Language.GLOBAL_WELCOME;
        SelectLanguageTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_LANGUAGE_GUI;
        SelectSavesFileTypeTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_SAVE_FILE_TYPE_GUI;
        SelectLogsFileTypeTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_LOGS_FILE_TYPE_GUI;
        SelectMaxFileSizeTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_MAX_FILE_SIZE_GUI;


        EncryptExtensionsTextBox.Text = MainApplication.Localization.Language.GLOBAL_ENCRYPT_EXTENSIONS_GUI;

        PriorityFilesTextBox.Text = MainApplication.Localization.Language.GLOBAL_PRIORITY_FILES_GUI;

        ButtonAddExtensionTextBlock.Content = MainApplication.Localization.Language.HOME_ADD_EXTENSION;

        ButtonAddFileTextBlock.Content = MainApplication.Localization.Language.HOME_ADD_PRIORITY_FILE;


        StatsTextBox.Text = MainApplication.Localization.Language.STATS_GLOBAL_MESSAGES;
        StatsListSavesTextBox.Text = MainApplication.Localization.Language.STATS_COUNT_SAVES;
        StatsListLogsTextBox.Text = MainApplication.Localization.Language.STATS_COUNT_LOGS;


    }

    private void ChangeCultureEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var language = comboBox.SelectedItem as CultureInfo;
        _homeViewModel.SelectedCultureInfo = language;

        MainWindow.GetInstance().InitTexts();

        InitTexts();
    }

    private void ChangeSavesFileTypeEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var saveFileType = (FileType)comboBox.SelectedItem;
        _homeViewModel.SelectedSavesFileType = saveFileType;
        _homeViewModel.UpdateStats();
    }

    private void ChangeLogsFileTypeEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var saveFileType = (FileType)comboBox.SelectedItem;
        _homeViewModel.SelectedLogsFileType = saveFileType;
        _homeViewModel.UpdateStats();
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

    public void MaxFileSizeNumberBox_ValueChanged(NumberBox sender,
                                            NumberBoxValueChangedEventArgs args)
    {

        if (double.IsNaN(args.NewValue) || args.NewValue <= 0)
        {
            sender.Value = sender.Minimum;
        } else {
            _homeViewModel.MaxFileSize = args.NewValue;
        }
    }
}



