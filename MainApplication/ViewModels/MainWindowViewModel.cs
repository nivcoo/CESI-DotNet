namespace MainApplication.ViewModels;

public class MainWindowViewModel : BaseViewModel
{

    public void InitSocket(bool isServer)
    {
        UIService.InitSocket(isServer);
    }
}