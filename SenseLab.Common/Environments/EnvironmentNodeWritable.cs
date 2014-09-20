using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Environments
{
    public class EnvironmentNodeWritable :
        NodeWritable<INode, IEnvironmentNode>,
        IEnvironmentNode
    {
        public EnvironmentNodeWritable(Guid id, string name, string description = null,
            INode parent = null, IList<IEnvironmentNode> children = null,
            IList<IRecordable> recordables = null)
            : base(id, name, description, parent, children)
        {
            if (recordables == null)
                recordables = new ObservableCollection<IRecordable>();
            Recordables = recordables;
        }
        
        public IList<IRecordable> Recordables { get; private set; }
        IEnumerable<IRecordable> IEnvironmentNode.Recordables
        {
            get { return Recordables; }
        }
    }
}
