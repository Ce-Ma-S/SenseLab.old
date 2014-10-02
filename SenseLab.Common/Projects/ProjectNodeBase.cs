using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SenseLab.Common.Projects
{
    [DataContract]
    public abstract class ProjectNodeBase :
        NodeWritable<ProjectNode>,
        INode<IProjectNode>,
        ILocatableChangeable<ISpatialLocation>
    {
        public ProjectNodeBase(Guid id, string name, string description = null,
            ISpatialLocation location = null)
            : base(id, name, description)
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
        IEnumerable<IProjectNode> INode<IProjectNode>.Children
        {
            get { return Children; }
        }

        private void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (LocationChanged != null)
                LocationChanged(this, new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }

        [DataMember(Name = "Location")]
        private ISpatialLocation location;
    }
}
