using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public abstract class EnvironmentNode<T> :
        Node<T>,
        IEnvironmentNode
        where T : IEnvironmentNode
    {
        public EnvironmentNode(Guid id, string name, string description = null)
            : base(id, name, description)
        {
            Recordables = new ObservableCollection<IRecordable>();
        }

        public abstract bool IsAvailable { get; }
        IEnumerable<IEnvironmentNode> INode<IEnvironmentNode>.Children
        {
            get { return Children.Cast<IEnvironmentNode>(); }
        }
        IEnumerable<IRecordable> IEnvironmentNode.Recordables
        {
            get { return Recordables; }
        }

        protected IList<IRecordable> Recordables { get; private set; }
    }
}
