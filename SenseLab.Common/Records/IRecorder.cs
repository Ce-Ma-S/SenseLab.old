namespace SenseLab.Common.Records
{
    public interface IRecorder : IRecords
    {
        bool Add(IRecord record);
        bool Remove(IRecord record);
        void Clear();
    }
}
