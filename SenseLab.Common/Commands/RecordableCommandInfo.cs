using SenseLab.Common.Records;
using System.Runtime.Serialization;

namespace SenseLab.Common.Commands
{
    [DataContract]
    public class RecordableCommandInfo :
        RecordSourceInfo
    {
        public RecordableCommandInfo(IRecordableCommand command)
            : base(command)
        {
            IsSynchronous = command.IsSynchronous;
        }

        public bool IsSynchronous { get; private set; }
    }
}
