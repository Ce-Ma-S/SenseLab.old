using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.ViewModels.Values
{
    public interface IValueViewModel<T> : IViewModel
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
}
