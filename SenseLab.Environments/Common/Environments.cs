using CeMaS.Common.Collections;
using SenseLab.Common.Records;
using System;
using System.Linq;

namespace SenseLab.Environments.Common
{
    public class Environments :
        IEnvironments
    {
        public INotifyList<IEnvironment, Guid> Items { get; } = new ObservableCollectionEx<IEnvironment, Guid>();

        IDevice IItemLookup<IDevice, Guid>.GetItem(Guid id)
        {
            return Items.
                Cast<IItemLookup<IDevice, Guid>>().
                GetItem(id);
        }
        bool IItemLookup<IDevice, Guid>.TryGetItem(Guid id, out IDevice item)
        {
            return Items.
                Cast<IItemLookup<IDevice, Guid>>().
                TryGetItem(id, out item);
        }
        bool IItemLookup<IDevice, Guid>.Contains(Guid id)
        {
            return Items.
                Cast<IItemLookup<IDevice, Guid>>().
                Contains(id);
        }

        IRecordable IItemLookup<IRecordable, Guid>.GetItem(Guid id)
        {
            return Items.
                Cast<IItemLookup<IRecordable, Guid>>().
                GetItem(id);
        }
        bool IItemLookup<IRecordable, Guid>.TryGetItem(Guid id, out IRecordable item)
        {
            return Items.
                Cast<IItemLookup<IRecordable, Guid>>().
                TryGetItem(id, out item);
        }
        bool IItemLookup<IRecordable, Guid>.Contains(Guid id)
        {
            return Items.
                Cast<IItemLookup<IRecordable, Guid>>().
                Contains(id);
        }
    }
}
