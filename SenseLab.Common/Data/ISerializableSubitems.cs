using System.Collections.Generic;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Supports serialization storage space optimization.
    /// </summary>
    public interface ISerializableSubitems
    {
        /// <summary>
        /// Info of subitems to be treated separately for storage space optimization.
        /// </summary>
        IEnumerable<SerializableItemInfo> SubitemInfos { get; }
        /// <summary>
        /// Access to a subitem to be treated separately for storage space optimization.
        /// </summary>
        /// <param name="subitemInfo">Info about a subitem.</param>
        /// <returns>Subitem.</returns>
        object this[SerializableItemInfo subitemInfo] { get; set; }
    }
}
