using SenseLab.Common.Data;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Project.
    /// </summary>
    public class Project :
        ProjectNodeBase,
        IProject
    {
        public Project(Guid id, string name, string description = null,
            /*INode parent = null,*/ IList<ProjectNode> children = null,
            ISpatialLocation location = null)
            : base(id, name, description, /*parent,*/ children, location)
        {
        }

        public IItemStorage<IRecord, KeyValuePair<Guid, uint>> Storage { get; private set; }
        public IList<IRecordTransformer> ReadRecordTransformers { get; private set; }
        public IList<IRecordTransformer> WriteRecordTransformers { get; private set; }
    }
}
