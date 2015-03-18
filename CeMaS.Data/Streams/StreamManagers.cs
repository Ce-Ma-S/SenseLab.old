using CeMaS.Common.Collections;
using System;

namespace CeMaS.Data.Streams
{
    public sealed class StreamManagers :
        ItemRegister<IStreamManager, Guid>
    {
        private StreamManagers()
        {
        }

        public static readonly StreamManagers Instance = new StreamManagers();
    }
}
