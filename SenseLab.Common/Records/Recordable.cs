using System;
using System.Threading.Tasks;

namespace SenseLab.Common.Records
{
    public abstract class Recordable<T> :
        RecordSource,
        IRecordable
        where T : IRecordProvider
    {
        public Recordable(Guid id, IRecordType type, string name, string description = null)
            : base(id, type, name, description)
        {
        }

        public abstract Task<T> CreateRecordProvider();
        async Task<IRecordProvider> IRecordable.CreateRecordProvider()
        {
            return await CreateRecordProvider();
        }
    }
}
