using MainApplication.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GuiApplication.Views.Dialogs;

public sealed partial class AddEncryptExtensionDialog : Page
{

    private readonly AddEncryptExtensionViewModel _addEncryptExtensionViewModel = new();

    private Uri SourcePath { get; set; }
    private Uri TargetPath { get; set; }

    public AddEncryptExtensionDialog()
    {
        InitializeComponent();
        DataContext = _addEncryptExtensionViewModel;
        InitTexts();
    }

    private void InitTexts()
    {
        EncryptExtensionName.PlaceholderText = MainApplication.Localization.Language.GLOBAL_NAME;
    }

    public void CreateButtonEvent(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string extensionName = EncryptExtensionName.Text;
        if (extensionName == null || extensionName.Length == 0)
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.CREATE_COMPLETE_ALL_INPUTS;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }

        if (_addEncryptExtensionViewModel.AlreadyEncryptExtensionWithSameName(extensionName))
        {
            ErrorInfoBar.Message = MainApplication.Localization.Language.EXTENSION_ADD_ASK_NAME_RETRY;
            ErrorInfoBar.Severity = InfoBarSeverity.Error;
            ErrorInfoBar.IsOpen = true;
            args.Cancel = true;
            return;
        }


        _addEncryptExtensionViewModel.AddNewEncryptExtension(extensionName);

    }
}

