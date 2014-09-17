namespace SenseLab.Common.Values
{
    public interface IValuePrecision<T> :
        IPrecision
    {
        T Value { get; }
    }
}
