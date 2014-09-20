using SenseLab.Common.Records;
using System.Windows.Input;

namespace SenseLab.Common.Commands
{
    public interface ICommandRecord :
        IRecord
    {
        ICommand Command { get; }
        object CommandParameter { get; }
    }
}
