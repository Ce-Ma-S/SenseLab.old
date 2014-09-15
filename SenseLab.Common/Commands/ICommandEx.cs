using System;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public interface ICommandEx :
        ICommand
    {
        string Name { get; }
        string Description { get; }
        event EventHandler<CommandExecutedEventArgs> Executed;
    }
}
