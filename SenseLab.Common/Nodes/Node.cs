using SenseLab.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace SenseLab.Common.Nodes
{
    [DataContract]
    public abstract class Node<T> :
        NotifyPropertyChange,
        INode<T>
        where T : INode
    {
        public Node(Guid id, string name, string description = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Children = new ObservableCollection<T>();
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

        IEnumerable<T> INode<T>.Children
        {
            get { return Children; }
        }
        IEnumerable<INode> INode.Children
        {
            get { return Children.Cast<INode>(); }
        }

        protected IList<T> Children { get; private set; }

        [DataMember(Name = "Children")]
        private IEnumerable<T> ChildrenSerialized
        {
            get { return Children; }
            set { Children = new ObservableCollection<T>(value); }
        }

        [DataMember(Name = "Name")]
        private string name;
        [DataMember(Name = "Description")]
        private string description;
    }
}
