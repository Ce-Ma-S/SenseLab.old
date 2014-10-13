using SenseLab.Common.Data;
using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Projects
{
    [DataContract]
    public abstract class ProjectNodeBase :
        NodeWritable<ProjectNode>,
        INode<IProjectNode>,
        ILocatableChangeable<ISpatialLocation>,
        IChangeAware
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
        public virtual bool IsChanged
        {
            get
            {
                return isChanged ||
                    Children.Any(node => node.IsChanged);
            }
            set
            {
                if (SetProperty(() => IsChanged, ref isChanged, value) && value)
                    OnChanged();
                foreach (var child in Children)
                    child.IsChanged = value;
            }
        }
        public event EventHandler Changed;

        protected override Node<ProjectNode> Clone()
        {
            var clone = (ProjectNodeBase)base.Clone();
            clone.isChanged = false;
            if (location != null)
                clone.location = location.Clone();
            return clone;
        }
        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            Changed = null;
        }

        protected virtual void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        protected override void OnChildrenChanged(object sender, ValueChangeEventArgs<IEnumerable<ProjectNode>> e)
        {
            base.OnChildrenChanged(sender, e);
            foreach (var child in e.OldValue.Value)
                child.Changed -= OnChildChanged;
            foreach (var child in e.NewValue)
                child.Changed += OnChildChanged;
        }

        private void OnChildChanged(object sender, EventArgs e)
        {
            IsChanged = true;
        }
        private void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (LocationChanged != null)
                LocationChanged(this, new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }

        [DataMember(Name = "Location")]
        private ISpatialLocation location;
        private bool isChanged;
    }
}
