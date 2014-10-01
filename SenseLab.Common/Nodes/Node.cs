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
        public Node(Guid id, string name, string description = null/*,
            P parent = default(P)*/)
        {
            Id = id;
            Name = name;
            Description = description;
            //Parent = parent;
            Children = new ObservableCollection<C>();
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

        protected IList<C> Children { get; private set; }

        [DataMember(Name = "Children")]
        private IEnumerable<C> ChildrenSerialized
        {
            get { return Children; }
            set { Children = new ObservableCollection<C>(value); }
        }

        [DataMember(Name = "Name")]
        private string name;
        [DataMember(Name = "Description")]
        private string description;
        //private P parent;
    }
}
