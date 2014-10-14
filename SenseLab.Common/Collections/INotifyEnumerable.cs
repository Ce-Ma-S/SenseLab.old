using System.Collections.Generic;

namespace SenseLab.Common.Collections
{
    public interface INotifyEnumerable<T> :
        IEnumerable<T>,
        INotifyItemContainmentChanged<T>
    {
    }
}
