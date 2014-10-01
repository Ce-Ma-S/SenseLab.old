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

        public void Register(TId id, TItem item)
        {
            items.Add(id, item);
        }
        public TId GetId(TItem item)
        {
            return items.First(i => i.Value == item).Key;
        }
        public TItem GetFromId(TId id)
        {
            return items[id];
        }

        private readonly Dictionary<TId, TItem> items = new Dictionary<TId, TItem>();
    }
}
