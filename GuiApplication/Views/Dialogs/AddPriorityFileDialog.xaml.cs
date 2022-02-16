using Microsoft.UI.Xaml.Controls;
using System;
using MainApplication.ViewModels;

namespace GuiApplication.Views.Dialogs;

public sealed partial class AddPriorityFileDialog : Page
{

    private readonly AddPriorityFileViewModel _addPriorityFileViewModel = new();

    private Uri SourcePath { get; set; }
    private Uri TargetPath { get; set; }

    public AddPriorityFileDialog()
    {
        InitializeComponent();
        DataContext = _addPriorityFileViewModel;
        InitTexts();
    }

    private void InitTexts()
    {
        PriorityFileName.PlaceholderText = MainApplication.Localization.Language.GLOBAL_NAME;
    }

    public void CreateButtonEvent(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string fileName = PriorityFileName.Text;
        if (fileName == null || fileName.Length == 0)
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.CREATE_COMPLETE_ALL_INPUTS;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }

        if (_addPriorityFileViewModel.AlreadyPriorityFileWithSameName(fileName))
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.EXTENSION_ADD_ASK_NAME_RETRY;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }


        _addPriorityFileViewModel.AddNewPriorityFile(fileName);

    }
}

