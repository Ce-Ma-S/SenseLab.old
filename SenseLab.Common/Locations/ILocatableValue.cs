namespace SenseLab.Common.Locations
{
    public interface ILocatableValue :
        ILocatable<ISpatialLocation>,
        ILocatable<ITimeInterval>
    {
        object Value { get; }
    }


    public interface ILocatableValue<T> :
        ILocatableValue
    {
        new T Value { get; }
    }
}
