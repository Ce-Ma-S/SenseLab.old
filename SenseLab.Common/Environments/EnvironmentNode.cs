using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public abstract class EnvironmentNode<T> :
        Node<INode, T>,
        IEnvironmentNode
        where T : IEnvironmentNode
    {
        public EnvironmentNode(Guid id, string name, string description = null,
            INode parent = null, IList<T> children = null,
            IList<IRecordable> recordables = null)
            : base(id, name, description, parent, children)
        {
            if (recordables == null)
                recordables = new ObservableCollection<IRecordable>();
            Recordables = recordables;
        }

        IEnumerable<IEnvironmentNode> INode<INode, IEnvironmentNode>.Children
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
