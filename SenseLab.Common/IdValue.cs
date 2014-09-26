namespace SenseLab.Common
{
    public class IdValue<TId, TValue> :
        IIdValue<TId, TValue>
    {
        public IdValue(TId id, TValue value)
        {
            Id = id;
            Value = value;
        }

        public TId Id { get; private set; }
        public TValue Value { get; private set; }

        public virtual IIdValue<TId, TValue> Clone(TId id)
        {
            var clone = (IdValue<TId, TValue>)MemberwiseClone();
            clone.Id = id;
            return clone;
        }
    }
}
