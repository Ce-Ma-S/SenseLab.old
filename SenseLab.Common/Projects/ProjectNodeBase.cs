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
            SetIsChangedOnChanged = true;
        }

        IEnumerable<IProjectNode> INode<IProjectNode>.Children
        {
            get { return Children; }
        }

        #region Location

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

        private void OnLocationChanged(ISpatialLocation oldValue, ISpatialLocation newValue)
        {
            if (LocationChanged != null)
                LocationChanged(this, new ValueChangeEventArgs<ISpatialLocation>(oldValue, newValue));
        }

        [DataMember(Name = "Location")]
        private ISpatialLocation location;

        #endregion

        #region IChangeAware

        public virtual bool IsChanged
        {
            get
            {
                return isChanged ||
                    Children.Any(node => node.IsChanged);
            }
            set
            {
                SetProperty(() => IsChanged, ref isChanged, value);
                if (!value)
                {
                    foreach (var child in Children)
                        child.IsChanged = value;
                }
            }
        }
        public event EventHandler Changed;

        protected bool SetIsChangedOnChanged { get; set; }

        protected virtual void OnChanged()
        {
            if (SetIsChangedOnChanged)
                IsChanged = true;
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        protected override void OnChildrenChanged(ValueChangeEventArgs<IEnumerable<ProjectNode>> e)
        {
            base.OnChildrenChanged(e);
            foreach (var child in e.OldValue.Value)
                child.Changed -= OnChildChanged;
            foreach (var child in e.NewValue)
                child.Changed += OnChildChanged;
            OnChanged();
        }
        protected virtual void OnChildChanged(object sender, EventArgs e)
        {
            OnChanged();
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName != "IsChanged")
                OnChanged();
        }
        [OnDeserialized]
        protected virtual void OnDeserialized()
        {
            SetIsChangedOnChanged = true;
        }

        private bool isChanged;

        #endregion

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
    }
}
