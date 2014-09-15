namespace SenseLab.Common.Records
{
    public interface IValueRecord :
        IRecord
    {
        object Value { get; }
    }
}
