using CeMaS.Common;
using CeMaS.Common.Events;
using CeMaS.Common.Validation;
using System;

namespace SenseLab.Common.Records
{
    public abstract class RecordSource :
        ItemInfo<Guid>,
        IRecordSource
    {
        public RecordSource(Guid id, IRecordType type, string name, string description = null)
            : base(id, name, description)
        {
            type.ValidateNonNull(nameof(type));
            Type = type;
        }

        #region Available

        public bool IsAvailable
        {
            get { return GetIsAvailable(); }
        }
        public event EventHandler IsAvailableChanged;
        public IRecordType Type
        {
            get; private set;
        }

        protected abstract bool GetIsAvailable();

        protected virtual void OnIsAvailableChanged()
        {
            IsAvailableChanged.RaiseEvent(this);
        }

        #endregion

        protected override void ClearEventHandlers()
        {
            base.ClearEventHandlers();
            IsAvailableChanged = null;
        }
    }
}
