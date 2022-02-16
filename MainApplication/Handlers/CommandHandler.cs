using System.Windows.Input;

namespace MainApplication.Handlers;

public class CommandHandler : ICommand
{
    private readonly Action<object?> _action;
    private readonly Func<object?, bool> _canExecute;


    /// <summary>
    ///     Creates instance of the command handler
    /// </summary>
    /// <param name="action">Action to be executed by the command</param>
    /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
    public CommandHandler(Action<object?> action, Func<object?, bool> canExecute)
    {
        _action = action;
        _canExecute = attr => canExecute.Invoke(this);
    }

    /// <summary>
    ///     Creates instance of the command handler
    /// </summary>
    /// <param name="action">Action to be executed by the command</param>
    public CommandHandler(Action<object?> action)
    {
        _action = action;
        _canExecute = obj => true;
    }

    /// <summary>
    ///     Wires CanExecuteChanged event
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    ///     Forcess checking if execute is allowed
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute.Invoke(parameter);
    }

    public void Execute(object? parameter)
    {
        _action(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }
}