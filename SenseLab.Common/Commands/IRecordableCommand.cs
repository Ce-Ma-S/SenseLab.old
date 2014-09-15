using SenseLab.Common.Records;
using System;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public interface IRecordableCommand :
        ICommand, IRecordable
    {
        event EventHandler<CommandExecutedEventArgs> Executed;
    }
}
