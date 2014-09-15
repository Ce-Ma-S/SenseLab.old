using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Values
{
    public interface IValue<T> : ILocatable
    {
        #region Read

        bool HasValue { get; }
        T Value { get; }
        event EventHandler ValueChanged;

        bool ReadValue();
        
        #endregion

        #region Write

        bool IsReadOnly { get; }
        T ValueWritable { get; set; }

        bool WriteValue();

        #endregion
    }

    //public interface IValue : IValue<object>
    //{
    //}
}
