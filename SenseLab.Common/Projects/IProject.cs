using SenseLab.Common.Data;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Lab project with selected nodes and recordables of <see cref="IEnvironment"/>.
    /// </summary>
    public interface IProject :
        INode</*INode,*/ IProjectNode>,
        ILocatable<ISpatialLocation>
    {
        /// <summary>
        /// Records recorded.
        /// </summary>
        IItemStorage<IRecord, KeyValuePair<Guid, uint>> Storage { get; }
        IList<IRecordTransformer> ReadRecordTransformers { get; }
        IList<IRecordTransformer> WriteRecordTransformers { get; }
    }
}
