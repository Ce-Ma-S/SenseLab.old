using CeMaS.Common.Commands;
using SenseLab.Common.Records;

namespace SenseLab.Common.Commands
{
    public interface IRecordableCommand :
        ICommandEx, IRecordable
    {
    }
}
