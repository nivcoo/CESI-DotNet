using MainApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;
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
    }

    private void InitTexts()
    {
        WelcomeTextBox.Text = MainApplication.Localization.Language.GLOBAL_WELCOME;
        SelectLanguageTextBox.Text = MainApplication.Localization.Language.GLOBAL_SELECT_LANGUAGE_GUI;
    }

    private void ChangeCultureEvent(object sender, SelectionChangedEventArgs args)
    {
        ComboBox comboBox = sender as ComboBox;
        var language = comboBox.SelectedItem as CultureInfo;
        _homeViewModel.SelectedCultureInfo = language;

        MainWindow.GetInstance().InitTexts();

        InitTexts();
    }
}



