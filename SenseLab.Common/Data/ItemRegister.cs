using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Data
{
    public class ItemRegister<TItem, TId>
        where TItem : class
    {
        public static readonly ItemRegister<TItem, TId> Instance = new ItemRegister<TItem, TId>();

        public ICollection<TId> Ids { get { return items.Keys; } }
        public ICollection<TItem> Items { get { return items.Values; } }

        public void Register(TItem item, TId id)
        {
            items.Add(id, item);
        }
        public TId GetId(TItem item)
        {
            return items.First(i => i.Value == item).Key;
        }
        public OptionalValue<TId> TryGetId(TItem item)
        {
            var p = items.SingleOrDefault(i => i.Value == item);
            return p.Value == null ?
                new OptionalValue<TId>() :
                new OptionalValue<TId>(p.Key);
        }
        public TItem GetFromId(TId id)
        {
            return items[id];
        }
        public TItem TryGetFromId(TId id)
        {
            TItem item;
            items.TryGetValue(id, out item);
            return item;
        }

        private readonly Dictionary<TId, TItem> items = new Dictionary<TId, TItem>();
    }


    public class ItemWithIdRegister<TItem, TId> :
        ItemRegister<TItem, TId>
        where TItem : class, IId<TId>
    {
        public void Register(TItem item)
        {
            Register(item, item.Id);
        }
    }
}
