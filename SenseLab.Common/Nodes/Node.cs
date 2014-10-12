using SenseLab.Common.Collections;
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
            children = new ObservableCollectionEx<T, Guid>();
            children.ItemContainmentChanged += OnChildrenChanged;
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

        protected IList<T> Children
        {
            get { return children; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (var child in Children.OfType<IDisposable>())
                    child.Dispose();
            }
        }
        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            children.ItemContainmentChanged -= OnChildrenChanged;
        }

        protected virtual void OnChildrenChanged(object sender, ValueChangeEventArgs<IEnumerable<T>> e)
        {
        }

        [DataMember(Name = "Children")]
        private IEnumerable<T> ChildrenSerialized
        {
            get { return children; }
            set
            {
                children = new ObservableCollectionEx<T, Guid>(value);
                children.ItemContainmentChanged += OnChildrenChanged;
            }
        }

        [DataMember(Name = "Name")]
        private string name;
        [DataMember(Name = "Description")]
        private string description;
        private ObservableCollectionEx<T, Guid> children;
    }
}
