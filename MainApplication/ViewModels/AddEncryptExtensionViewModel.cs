using MainApplication.Services;

namespace MainApplication.ViewModels;

public class AddEncryptExtensionViewModel : BaseViewModel
{

    public bool AddNewEncryptExtension(string extensionName)
    {
        return ConfigurationService.AddEncryptExtension(extensionName);
    }

    public bool AlreadyEncryptExtensionWithSameName(string extensionName)
    {
        return ConfigurationService.AlreadyEncryptExtensionWithSameName(extensionName);
    }
}