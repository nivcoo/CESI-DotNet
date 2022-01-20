﻿using System.Windows.Input;
namespace MainApplication.Handlers;

public class CommandHandler : ICommand
{
    private readonly Action _action;
    private readonly Func<bool> _canExecute;

    /// <summary>
    /// Creates instance of the command handler
    /// </summary>
    /// <param name="action">Action to be executed by the command</param>
    /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
    public CommandHandler(Action action, Func<bool> canExecute)
    {
        _action = action;
        _canExecute = canExecute;
    }

    /// <summary>
    /// Wires CanExecuteChanged event 
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add {  }
        remove { }
    }

    /// <summary>
    /// Forcess checking if execute is allowed
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute.Invoke();
    }

    public void Execute(object? parameter)
    {
        _action();
    }
}