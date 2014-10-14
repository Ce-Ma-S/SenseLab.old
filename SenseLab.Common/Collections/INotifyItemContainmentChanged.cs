using SenseLab.Common.Events;
using System;
using System.Collections.Generic;

namespace SenseLab.Common.Collections
{
    public interface INotifyItemContainmentChanged<T>
    {
        event EventHandler<ValueChangeEventArgs<IEnumerable<T>>> ItemContainmentChanged;
    }
}
