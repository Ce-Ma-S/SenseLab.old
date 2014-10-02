using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public abstract class NodeWritable<T> :
        Node<T>
        where T : INode
    {
        public NodeWritable(Guid id, string name, string description = null)
            : base(id, name, description)
        {
        }

        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        public new string Description
        {
            get { return base.Description; }
            set { base.Description = value; }
        }

        public new IList<T> Children
        {
            get { return base.Children; }
        }
    }
}
