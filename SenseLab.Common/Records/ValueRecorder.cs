﻿using SenseLab.Common.Events;
using SenseLab.Common.Values;
using System;

namespace SenseLab.Common.Records
{
    public class ValueRecorder<T> :
        Recorder<ValueRecord<T>>
    {
        public ValueRecorder(IValue<T> value)
        {
            value.ValidateNonNull("value");
            Value = value;
        }

        public IValue<T> Value { get; private set; }
        public override IRecordable Recordable
        {
            get { return Value; }
        }

        protected override void DoStart()
        {
            Value.ValueChanged += OnValueChanged;
        }
        protected override void DoStop()
        {
            Value.ValueChanged -= OnValueChanged;
        }
        protected override void DoPause()
        {
            DoStop();
        }
        protected override void DoUnpause()
        {
            DoStart();
        }
        protected override ValueRecord<T> CreateRecord(object data)
        {
            var a = (ValueChangeEventArgs<T>)data;;
            return new ValueRecord<T>(a.NewValue, Recordable);
        }

        private void OnValueChanged(object sender, ValueChangeEventArgs<T> e)
        {
            AddRecord(e);
        }
    }
}
