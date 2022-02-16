namespace MainApplication.ViewModels;

public class AddPriorityFileViewModel : BaseViewModel
{
    public bool AddNewPriorityFile(string fileName)
    {
        return ConfigurationService.AddPriorityFile(fileName);
    }

    public bool AlreadyPriorityFileWithSameName(string fileName)
    {
        return ConfigurationService.AlreadyPriorityFileWithSameName(fileName);
    }
}