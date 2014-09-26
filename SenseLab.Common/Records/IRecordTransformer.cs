namespace SenseLab.Common.Records
{
    public interface IRecordTransformer
    {
        bool CanTransform(IRecord record);
        IRecord Transform(IRecord record);
    }
}
