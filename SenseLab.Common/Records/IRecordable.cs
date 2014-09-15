using System;

namespace SenseLab.Common.Records
{
    public interface IRecordable
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        IRecorder Recorder { get; }
    }
}
