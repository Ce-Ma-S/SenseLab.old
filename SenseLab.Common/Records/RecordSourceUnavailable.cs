using System;

namespace SenseLab.Common.Records
{
    public class RecordSourceUnavailable :
        RecordSource
    {
        public RecordSourceUnavailable(RecordSourceInfo sourceInfo)
            : base(sourceInfo.Id,
            new RecordType(sourceInfo.Type.Id, sourceInfo.Type.Name, sourceInfo.Type.Description),
            sourceInfo.Name, sourceInfo.Description)
        {
        }

        public override bool IsAvailable
        {
            get { return false; }
        }
    }
}
