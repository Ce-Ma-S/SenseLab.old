using SenseLab.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Nodes
{
    [DataContract]
    public abstract class Node</*P,*/ C> :
        NotifyPropertyChange,
        INode</*P,*/ C>
        //where P : INode
        where C : INode
    {
        public Node(Guid id, string name, string description = null,
            /*P parent = default(P),*/ IList<C> children = null)
        {
            Id = id;
            Name = name;
            Description = description;
            //Parent = parent;
            if (children == null)
                children = new ObservableCollection<C>();
            Children = children;
        }

        [DataMember]
        public Guid Id { get; private set; }
        public string Name
        {
            get { return name; }
            protected set
            {
                name.ValidateNonNullOrEmpty("name");
                SetProperty(() => Name, ref name, value);
            }
        }
        public string Description
        {
            get { return description; }
            protected set
            {
                SetProperty(() => Description, ref description, value);
            }
        }

        //public P Parent
        //{
        //    get { return parent; }
        //    protected set
        //    {
        //        SetProperty(() => Parent, ref parent, value);
        //    }
        //}
        //INode INode.Parent
        //{
        //    get { return Parent; }
        //}
        IEnumerable<C> INode</*P,*/ C>.Children
        {
            get { return Children; }
        }
        IEnumerable<INode> INode.Children
        {
            get { return Children.Cast<INode>(); }
        }

        [DataMember]
        protected IList<C> Children { get; private set; }

        [DataMember(Name = "Name")]
        private string name;
        [DataMember(Name = "Description")]
        private string description;
        //private P parent;
    }
}
