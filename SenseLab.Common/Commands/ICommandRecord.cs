using SenseLab.Common.Records;

namespace SenseLab.Common.Commands
{
    public interface ICommandRecord :
        IRecord
    {
        object CommandParameter { get; }
    }
}
