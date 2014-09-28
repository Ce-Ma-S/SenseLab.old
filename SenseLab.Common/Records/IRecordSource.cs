using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Record source.
    /// </summary>
    public interface IRecordSource :
        IId<Guid>
    {
        /// <summary>
        /// Name.
        /// </summary>
        /// <value>non-empty</value>
        string Name { get; }
        /// <summary>
        /// Description.
        /// </summary>
        /// <value>optional</value>
        string Description { get; }
        /// <summary>
        /// Record type.
        /// </summary>
        IRecordType Type { get; }
    }
}
