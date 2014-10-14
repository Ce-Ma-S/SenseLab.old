using SenseLab.Common.Collections;
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
            recordables = new ObservableCollectionEx<IRecordable>();
        }

        #region IsAvailable

        public abstract bool IsAvailable { get; }

        protected virtual void OnIsAvailableChanged()
        {
            OnPropertyChanged(() => IsAvailable);
        }

        #endregion

        IEnumerable<IEnvironmentNode> INode<IEnvironmentNode>.Children
        {
            get { return Children.Cast<IEnvironmentNode>(); }
        }
        INotifyEnumerable<IRecordable> IEnvironmentNode.Recordables
        {
            get { return recordables; }
        }

        protected IList<IRecordable> Recordables
        {
            get { return recordables; }
        }

        private ObservableCollectionEx<IRecordable> recordables;
    }
}
