using MainApplication.Objects.Enums;
using MainApplication.ViewModels;
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

        SaveFileTypeComboBox.SelectedItem = _homeViewModel.SelectedSaveFileType;
        SaveFileTypeComboBox.SelectionChanged += ChangeSaveFileTypeEvent;
    }

    private void InitTexts()
    {
        WelcomeTextBox.Text = MainApplication.Localization.Language.GLOBAL_WELCOME;
        SelectLanguageTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_LANGUAGE_GUI;
        SelectSaveFileTypeTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_SAVE_FILE_TYPE_GUI;
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
        var saveFileType = (SaveFileType) comboBox.SelectedItem;
        _homeViewModel.SelectedSaveFileType = saveFileType;
    }
}



