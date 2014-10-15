using Microsoft.Practices.ServiceLocation;
using SenseLab.Common.Collections;
using SenseLab.Common.Environments;
using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Projects
{
    /// <summary>
    /// Wraps another node to be in a project.
    /// </summary>
    [DataContract]
    public class ProjectNode :
        ProjectNodeBase,
        IProjectNode
    {
        public ProjectNode(Guid id, string name, string description = null,
            ISpatialLocation location = null)
            : base(id, name, description, location)
        {
            enabledRecordables = new ObservableCollectionEx<IRecordable, Guid>();
        }
        public ProjectNode(IEnvironmentNode node,
            ISpatialLocation location = null)
            : this(node.Id, node.Name, node.Description, location)
        {
            Node = node;
            try
            {
                SetIsChangedOnChanged = false;
                FillChildrenFromNode();
            }
            finally
            {
                SetIsChangedOnChanged = true;
            }
        }

        #region Node

        /// <summary>
        /// Node this wrapper works with.
        /// </summary>
        public IEnvironmentNode Node { get; private set; }

        [DataMember]
        private NodeInfo NodeInfo
        {
            get
            {
                if (Node == null)
                    return null;
                return new NodeInfo(Node);
            }
            set
            {
                if (value == null)
                    return;
                Node = EnvironmentHelper.NodeFromId(value.Id);
                if (Node == null)
                    Node = new EnvironmentNodeUnavailable(value);
                else
                {
                    ((INode)Node).Children.ItemContainmentChanged += OnNodeChildrenChanged;
                    Node.Recordables.ItemContainmentChanged += OnNodeRecordablesChanged;
                }
            }
        }

        private void FillChildrenFromNode()
        {
            FillChildrenFrom(Node.Children);
            ((INode)Node).Children.ItemContainmentChanged += OnNodeChildrenChanged;
            FillEnabledRecordablesFromNode();
        }

        private void FillChildrenFrom(IEnumerable<IEnvironmentNode> children)
        {
            var nodes = children.Select(child => new ProjectNode(child));
            Children.Add(nodes);
        }

        private void OnNodeChildrenChanged(object sender, ValueChangeEventArgs<IEnumerable<INode>> e)
        {
            var nodes = Children.Where(child => child.Node != null && e.OldValue.Value.Contains(child.Node)).ToArray();
            Children.Remove(nodes);
            FillChildrenFrom(e.NewValue.Cast<IEnvironmentNode>());
        }

        #endregion

        #region IsEnabled

        /// <summary>
        /// Whether <see cref="Node"/> is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (SetProperty(() => IsEnabled, ref isEnabled, value, OnIsEnabledChanged))
                {
                    foreach (var child in Children)
                    {
                        child.IsEnabled = value;
                    }
                }
            }
        }
        public event EventHandler IsEnabledChanged;

        protected virtual void OnIsEnabledChanged()
        {
            if (IsEnabledChanged != null)
            {
                IsEnabledChanged(this, EventArgs.Empty);
            }
        }

        [DataMember(Name = "IsEnabled")]
        private bool isEnabled;

        #endregion

        #region EnabledRecordables

        public IList<IRecordable> EnabledRecordables
        {
            get { return enabledRecordables; }
        }
        INotifyEnumerable<IRecordable> IProjectNode.EnabledRecordables
        {
            get { return enabledRecordables; }
        }

        private void FillEnabledRecordablesFromNode()
        {
            FillEnabledRecordablesFrom(Node.Recordables);
            Node.Recordables.ItemContainmentChanged += OnNodeRecordablesChanged;
        }
        private void FillEnabledRecordablesFrom(IEnumerable<IRecordable> recordables)
        {
            enabledRecordables.Add(recordables);
        }

        private void OnNodeRecordablesChanged(object sender, ValueChangeEventArgs<IEnumerable<IRecordable>> e)
        {
            enabledRecordables.Remove(e.OldValue.Value);
            FillEnabledRecordablesFrom(e.NewValue);
        }

        [DataMember]
        private IEnumerable<Guid> EnabledRecordableIds
        {
            get
            {
                // keep enabled recordable ids of unavailable node
                if (Node is EnvironmentNodeUnavailable)
                    return ((EnvironmentNodeUnavailable)Node).RecordableIds;
                return EnabledRecordables.Select(r => r.Id);
            }
            set
            {
                // keep enabled recordable ids of unavailable node
                if (Node is EnvironmentNodeUnavailable)
                {
                    ((EnvironmentNodeUnavailable)Node).RecordableIds = value;
                }
                else
                {
                    var recordables = Node.RecordablesFromIds(value);
                    enabledRecordables = new ObservableCollectionEx<IRecordable, Guid>(recordables);
                }
            }
        }

        private ObservableCollectionEx<IRecordable, Guid> enabledRecordables;

        #endregion

        protected override Node<ProjectNode> Clone()
        {
            var clone = (ProjectNode)base.Clone();
            if (Node != null && !(Node is EnvironmentNodeUnavailable))
            {
                ((INode)Node).Children.ItemContainmentChanged += OnNodeChildrenChanged;
                Node.Recordables.ItemContainmentChanged += OnNodeRecordablesChanged;
            }
            clone.enabledRecordables = new ObservableCollectionEx<IRecordable, Guid>(enabledRecordables);
            return clone;
        }
        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            if (Node != null && !(Node is EnvironmentNodeUnavailable))
            {
                ((INode)Node).Children.ItemContainmentChanged -= OnNodeChildrenChanged;
                Node.Recordables.ItemContainmentChanged -= OnNodeRecordablesChanged;
            }
        }
    }
}
