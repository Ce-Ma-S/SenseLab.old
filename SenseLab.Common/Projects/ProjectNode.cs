using SenseLab.Common.Environments;
using SenseLab.Common.Events;
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
        NodeWritable<INode, ProjectNode>,
        IProjectNode,
        ILocatableChangeable<ISpatialLocation>
    {
        public ProjectNode(Guid id, string name, string description = null,
            INode parent = null, IList<ProjectNode> children = null,
            ISpatialLocation location = null)
            : base(id, name, description, parent, children)
        {
            Location = location;
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
        
        public ISpatialLocation Location
        {
            get { return location; }
            set
            {
                SetProperty(() => Location, ref location, value, OnLocationChanged);
            }
        }

        private void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (LocationChanged != null)
                LocationChanged(this, new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }
        /// <summary>
        /// Fired when <see cref="ILocatable{T}.Location"/> is changed.
        /// </summary>
        public event EventHandler<ValueChangeEventArgs<ISpatialLocation>> LocationChanged;
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
        IEnumerable<IProjectNode> INode<INode, IProjectNode>.Children
        {
            get { return Children; }
        }

        private IEnvironmentNode node;
        private bool isSelected;
        private ISpatialLocation location;
    }
}
