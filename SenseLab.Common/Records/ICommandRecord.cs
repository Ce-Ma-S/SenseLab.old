using System.Windows.Input;

namespace SenseLab.Common.Records
{
    public interface ICommandRecord :
        IRecord
    {
        ICommand Command { get; }
        object CommandParameter { get; }
    }
}
