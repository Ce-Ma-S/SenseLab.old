using CeMaS.Common.Events;
using SenseLab.Common.Records;
using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Values
{
    public interface IValue<T> :
        IRecordable
    {
        #region Read

        bool HasValue { get; }
        T Value { get; }
        event EventHandler<ValueChangeEventArgs<T>> ValueChanged;

        Task<bool> ReadValue();
        
        #endregion

        #region Write

        bool IsReadOnly { get; }
        T ValueWritable { get; set; }

        Task<bool> WriteValue();

        #endregion
    }
}
