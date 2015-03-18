using CeMaS.Common.Events;
using System.Runtime.Serialization;

namespace SenseLab.Common.Locations
{
    /// <summary>
    /// Location.
    /// </summary>
    public interface ILocation :
        IChangeable
    {
        /// <summary>
        /// Clones this location.
        /// </summary>
        /// <returns>Independent clone.</returns>
        ILocation Clone();
    }
}
