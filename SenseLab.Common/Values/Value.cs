using CeMaS.Common.Events;
using SenseLab.Common.Events;
using SenseLab.Common.Locations;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Common.Values
{
    public abstract class Value<T> :
        Recordable<ValueRecordProvider<T>>,
        IValue<T>
    {
        public Value(Guid id, IRecordType type, string name, string description = null)
            : base(id, type, name, description)
        {
        }

        public bool HasValue
        {
            get { throw new NotImplementedException(); }
        }
        T IValue<T>.Value
        {
            get { throw new NotImplementedException(); }
        }
        public event EventHandler<ValueChangeEventArgs<T>> ValueChanged;

        public bool ReadValue()
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
        public T ValueWritable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool WriteValue()
        {
            throw new NotImplementedException();
        }

        public virtual void OnValueChanged(T newValue, ILocation location)
        {
            ValueChanged.RaiseEvent(this, () => new ValueChangeEventArgs<T>(newValue, location));
        }
    }
}
