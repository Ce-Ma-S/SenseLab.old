using SenseLab.Common.Quantities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.ViewModels.Values
{
    public interface IPhysicalValueViewModel<T> : IValueViewModel<T>
    {
        Quantity Quantity { get; }
        Unit Unit { get; }
    }
}
