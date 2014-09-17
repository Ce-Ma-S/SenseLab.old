﻿using System;

namespace SenseLab.Common.Records
{
    public interface IRecordable :
        IId<Guid>
    {
        Guid Id { get; }
        IRecordType Type { get; }
        IRecorder Recorder { get; }
    }
}
