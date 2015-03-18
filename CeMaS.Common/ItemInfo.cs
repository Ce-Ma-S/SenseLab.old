using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;
using System.Runtime.Serialization;

namespace CeMaS.Common
{
    [DataContract]
    public class ItemInfo<T> :
        NotifyPropertyChange,
        IItemInfo<T>
    {
        public ItemInfo(T id, string name, string description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        [DataMember]
        public T Id { get; protected set; }

        #region Name

        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                value.ValidateNonNullOrEmpty(nameof(Name));
                SetProperty(() => Name, ref name, value, OnNameChanged);
            }
        }
        public event EventHandler NameChanged;

        protected virtual void OnNameChanged()
        {
            NameChanged.RaiseEvent(this);
        }

        private string name;

        #endregion

        #region Description

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetProperty(() => Description, ref description, value, OnDescriptionChanged);
            }
        }
        public event EventHandler DescriptionChanged;

        protected virtual void OnDescriptionChanged()
        {
            DescriptionChanged.RaiseEvent(this);
        }

        private string description;

        #endregion

        public override string ToString()
        {
            return string.Format("{0}\n({1})\n{2}", Name, Id, Description);
        }

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            NameChanged = null;
            DescriptionChanged = null;
        }
    }
}
