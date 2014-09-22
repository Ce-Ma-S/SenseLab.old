namespace SenseLab.Common.Locations
{
    /// <summary>
    /// Allows specification of a location of a locatable object.
    /// </summary>
    /// <typeparam name="T">Location type.</typeparam>
    public interface ILocatable<T>
        where T : ILocation
    {
        /// <summary>
        /// Location.
        /// </summary>
        /// <value>Can be null.</value>
        T Location { get; }
    }
}
