namespace SenseLab.Common.Records
{
    public class RecordSourceUnavailable :
        RecordSource
    {
        public RecordSourceUnavailable(RecordSourceInfo sourceInfo)
            : base(sourceInfo.Id, sourceInfo.Type, sourceInfo.Name, sourceInfo.Description)
        {
        }

        protected override bool GetIsAvailable()
        {
            return false;
        }
    }
}
