using CeMaS.Common.Collections;
using System;

namespace CeMaS.Data.Storages
{
    public sealed class Storages :
        ItemRegister<IStorage, Guid>
    {
        private Storages()
        {
        }

        public static readonly Storages Instance = new Storages();
    }
}
