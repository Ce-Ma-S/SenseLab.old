using System;
using System.Collections.Generic;

namespace SenseLab.Common.Nodes
{
    public abstract class NodeWritable</*P,*/ C> :
        Node</*P,*/ C>
        //where P : INode
        where C : INode
    {
        public NodeWritable(Guid id, string name, string description = null/*,
            P parent = default(P)*/)
            : base(id, name, description/*, parent*/)
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

        //public new P Parent
        //{
        //    get { return base.Parent; }
        //    set { base.Parent = value; }
        //}
        public new IList<C> Children
        {
            get { return base.Children; }
        }
    }
}
