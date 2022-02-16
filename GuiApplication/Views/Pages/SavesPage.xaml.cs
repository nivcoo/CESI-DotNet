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
    public string SaveSourcePathTextBlock { get; set; }
    public string SaveTargetPathTextBlock { get; set; }

    public string SaveTypeTextBlock { get; set; }

    public string SaveProgressionTextBlock { get; set; }

    public string ButtonCreateSaveTextBlock { get; set; }

    public string ButtonStartAllSavesTextBlock { get; set; }

    public string ButtonPauseAllSavesTextBlock { get; set; }

    public string ButtonResumeAllSavesTextBlock { get; set; }

    public SavesPage()
    {
        DataContext = _saveViewModel;

        if (_saveViewModel.DispatchUiAction == null)
        {
            DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _saveViewModel.DispatchUiAction = (action) => dispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () => { action.Invoke(); });
        }
        InitializeComponent();

        InitTexts();

        _saveViewModel.SavePageErrorAction += SaveError;
    }

    private void InitTexts()
    {
        SaveSourcePathTextBlock = MainApplication.Localization.Language.GLOBAL_SOURCE;
        SaveTargetPathTextBlock = MainApplication.Localization.Language.GLOBAL_TARGET;
        SaveTypeTextBlock = MainApplication.Localization.Language.GLOBAL_TYPE;
        SaveProgressionTextBlock = MainApplication.Localization.Language.GLOBAL_PROGRESSION;

        ButtonCreateSaveTextBlock = MainApplication.Localization.Language.BUTTON_CREATE_SAVE;
        ButtonStartAllSavesTextBlock = MainApplication.Localization.Language.BUTTON_START_ALL_SAVES;
        ButtonPauseAllSavesTextBlock = MainApplication.Localization.Language.BUTTON_PAUSE_ALL_SAVES;
        ButtonResumeAllSavesTextBlock = MainApplication.Localization.Language.BUTTON_RESUME_ALL_SAVES;
    }

    private async void CreateSave_OpenDialog(object sender, RoutedEventArgs e)
    {
        var content = new CreateSaveDialog();
        ContentDialog dialog = new()
        {
            Title = MainApplication.Localization.Language.BUTTON_CREATE_SAVE,
            PrimaryButtonText = MainApplication.Localization.Language.GLOBAL_CREATE,
            CloseButtonText = MainApplication.Localization.Language.GLOBAL_CANCEL,
            DefaultButton = ContentDialogButton.Primary,
            Content = content
        };
        dialog.XamlRoot = this.Content.XamlRoot;
        dialog.PrimaryButtonClick += content.CreateButtonEvent;
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
            _saveViewModel.UpdateSavesList();

    }
    private void SaveError(string errorMessage)
    {

        if (errorMessage == string.Empty)
        {
            SaveInfoBar.IsOpen = false;
        }
        else
        {
            SaveInfoBar.Message = errorMessage;
            SaveInfoBar.Severity = InfoBarSeverity.Error;
            SaveInfoBar.IsOpen = true;
        }
    }
}

