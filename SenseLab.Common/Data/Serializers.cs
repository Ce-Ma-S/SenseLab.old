using System;

namespace SenseLab.Common.Data
{
    public class Serializers<T> :
        ItemRegister<ISerializer<T>, Guid>
    {
    }
}
