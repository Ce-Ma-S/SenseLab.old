using System;

namespace SenseLab.Common.Records
{
    public interface IRecordable :
        IId<Guid>
    {
        IRecordType Type { get; }

        IRecorder CreateRecorder();
    }
}
