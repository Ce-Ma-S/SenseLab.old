using Microsoft.Practices.ServiceLocation;
using SenseLab.Common.Environments;
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

        [DataMember]
        private IdNameDescription<Guid> NodeInfo
        {
            get
            {
                if (Node == null)
                    return null;
                return new IdNameDescription<Guid>(Node.Id, Node.Name, Node.Description);
            }
            set
            {
                if (value == null)
                    return;
                node = (IEnvironmentNode)environment.FromId(value.Id);
                if (node == null)
                    node = new EnvironmentNodeUnavailable(value.Id, value.Name, value.Description);
            }
        }
        [DataMember]
        private IEnumerable<Guid> SelectedRecordableIds
        {
            get
            {
                // keep selected recordable ids of unavailable node
                if (node is EnvironmentNodeUnavailable)
                    return ((EnvironmentNodeUnavailable)node).RecordableIds;
                return SelectedRecordables.Select(r => r.Id);
            }
            set
            {
                // keep selected recordable ids of unavailable node
                if (node is EnvironmentNodeUnavailable)
                {
                    ((EnvironmentNodeUnavailable)node).RecordableIds = value;
                }
                else
                {
                    var recordables = node.RecordablesFromIds(value);
                    SelectedRecordables = new ObservableCollection<IRecordable>(recordables);
                }
            }
        }

        private static readonly IEnvironment environment = ServiceLocator.Current.GetInstance<IEnvironment>();

        private IEnvironmentNode node;
        [DataMember(Name = "IsSelected")]
        private bool isSelected;
    }
}
