using System;
using System.Windows.Input;

namespace CeMaS.Common.Commands
{
    /// <summary>
    /// Extended command.
    /// </summary>
    public interface ICommandEx :
        ICommand
    {
        bool IsSynchronous { get; }
        
        event EventHandler<CommandExecutedEventArgs> Executed;
    }
}
