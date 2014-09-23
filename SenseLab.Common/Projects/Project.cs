using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            INode parent = null, IList<ProjectNode> children = null,
            ISpatialLocation location = null)
            : base(id, name, description, parent, children, location)
        {
            Records = new ObservableCollection<IRecords>();
        }

        public IList<IRecords> Records { get; private set; }
        IEnumerable<IRecords> IProject.Records
        {
            get { return Records; }
        }
    }
}
