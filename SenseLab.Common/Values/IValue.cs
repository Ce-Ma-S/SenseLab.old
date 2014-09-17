﻿using SenseLab.Common.Events;
using SenseLab.Common.Records;
using System;

namespace SenseLab.Common.Values
{
    public interface IValue<T> :
        IRecordable
    {
        #region Read

        bool HasValue { get; }
        T Value { get; }
        event EventHandler<ValueChangeEventArgs<T>> ValueChanged;

        bool ReadValue();
        
        #endregion

        #region Write

        bool IsReadOnly { get; }
        T ValueWritable { get; set; }

        bool WriteValue();

        #endregion
    }
}
