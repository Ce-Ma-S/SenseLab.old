using SenseLab.Common.Records;

namespace SenseLab.Common.Commands
{
    public interface ICommandRecord :
        IRecord
    {
        IRecordableCommand Command { get; }
        object CommandParameter { get; }
    }
}
