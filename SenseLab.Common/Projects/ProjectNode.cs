using SenseLab.Common.Environments;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Microsoft.Practices.ServiceLocation;
using SenseLab.Common.Nodes;

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
            //INode parent = null,
            ISpatialLocation location = null)
            : base(id, name, description, /*parent,*/ location)
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
        private Guid NodeId
        {
            get { return Node.Id; }
            set { node = (IEnvironmentNode)environment.FromId(value); }
        }
        [DataMember]
        private IEnumerable<Guid> SelectedRecordableIds
        {
            get { return SelectedRecordables.Select(r => r.Id); }
            set
            {
                var recordables = node.RecordablesFromIds(value);
                SelectedRecordables = new ObservableCollection<IRecordable>(recordables);
            }
        }

        private static readonly IEnvironment environment = ServiceLocator.Current.GetInstance<IEnvironment>();

        private IEnvironmentNode node;
        [DataMember(Name = "IsSelected")]
        private bool isSelected;
    }
}
