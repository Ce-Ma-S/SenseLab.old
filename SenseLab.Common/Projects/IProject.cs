using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Lab project with selected nodes and recordables of <see cref="IEnvironment"/>.
    /// </summary>
    public interface IProject :
        INode<IProjectNode>,
        ILocatable<ISpatialLocation>
    {
        /// <summary>
        /// Records recorded.
        /// </summary>
        IRecordStorage Records { get; }
        //IList<IRecordTransformer> ReadRecordTransformers { get; }
        //IList<IRecordTransformer> WriteRecordTransformers { get; }

        /// <summary>
        /// Clones this project without <see cref="Records"/>.
        /// </summary>
        /// <param name="createRecords">Creates <see cref="Records"/> for the clone.</param>
        Task<IProject> Clone(Func<Guid, Task<IRecordStorage>> createRecords);
    }
}
