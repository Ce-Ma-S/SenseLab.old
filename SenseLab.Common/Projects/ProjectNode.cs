using SenseLab.Common.Environments;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Wraps another node to be in a project.
    /// </summary>
    public class ProjectNode :
        ProjectNodeBase,
        IProjectNode
    {
        public ProjectNode(Guid id, string name, string description = null,
            INode parent = null, IList<ProjectNode> children = null,
            ISpatialLocation location = null)
            : base(id, name, description, parent, children, location)
        {
            SelectedRecordables = new ObservableCollection<IRecordable>();
        }

        /// <summary>
        /// Node this wrapper works with.
        /// </summary>
        public IEnvironmentNode Node
        {
            get { return node; }
            set
            {
                SetProperty(() => Node, ref node, value);
            }
        }
        /// <summary>
        /// Whether <see cref="Node"/> is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (SetProperty(() => IsSelected, ref isSelected, value))
                {
                    foreach (var child in Children)
                    {
                        child.IsSelected = value;
                    }
                }
            }
        }
        public IList<IRecordable> SelectedRecordables { get; private set; }
        IEnumerable<IRecordable> IProjectNode.SelectedRecordables
        {
            get { return SelectedRecordables; }
        }

        private IEnvironmentNode node;
        private bool isSelected;
    }
}
