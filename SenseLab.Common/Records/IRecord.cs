using CeMaS.Common;
using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Records
{
    /// <summary>
    /// Record provided by <see cref="IRecordSource"/>.
    /// </summary>
    /// <remarks><see cref="ITimeInterval"/> is required.</remarks>
    public interface IRecord :
        IIdWritable<uint>,
        ILocatable<ISpatialLocation>,
        ILocatable<ITimeInterval>
    {
        /// <summary>
        /// Record source identifier.
        /// </summary>
        Guid SourceId { get; }
    }
}
