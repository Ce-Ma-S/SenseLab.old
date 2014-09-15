namespace SenseLab.Common.Locations
{
    public interface ILocatable<T>
        where T : ILocation
    {
        T Location { get; }
    }
}
