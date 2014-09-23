using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Projects
{
    public abstract class ProjectNodeBase :
        NodeWritable<INode, ProjectNode>,
        INode<INode, IProjectNode>,
        ILocatableChangeable<ISpatialLocation>
    {
        public ProjectNodeBase(Guid id, string name, string description = null,
            INode parent = null, IList<ProjectNode> children = null,
            ISpatialLocation location = null)
            : base(id, name, description, parent, children)
        {
            Location = location;
        }

        public ISpatialLocation Location
        {
            get { return location; }
            set
            {
                SetProperty(() => Location, ref location, value, OnLocationChanged);
            }
        }
        /// <summary>
        /// Fired when <see cref="ILocatable{T}.Location"/> is changed.
        /// </summary>
        public event EventHandler<ValueChangeEventArgs<ISpatialLocation>> LocationChanged;
        IEnumerable<IProjectNode> INode<INode, IProjectNode>.Children
        {
            get { return Children; }
        }

        private void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (LocationChanged != null)
                LocationChanged(this, new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }

        private ISpatialLocation location;
    }
}
