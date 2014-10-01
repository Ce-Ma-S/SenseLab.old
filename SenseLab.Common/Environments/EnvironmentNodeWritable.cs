using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SenseLab.Common.Environments
{
    [DataContract]
    public class EnvironmentNodeWritable :
        NodeWritable</*INode,*/ IEnvironmentNode>,
        IEnvironmentNode
    {
        public EnvironmentNodeWritable(Guid id, string name, string description = null,
            //INode parent = null,
            IList<IRecordable> recordables = null)
            : base(id, name, description/*, parent*/)
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
