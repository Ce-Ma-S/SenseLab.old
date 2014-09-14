namespace SenseLab.Common.Locations
{
    public interface ILocatable<T>
        where T : ILocation
    {
        T Location { get; }
    }


    public interface ILocatable : ILocatable<ILocation>
    {
    }
}
