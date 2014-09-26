namespace SenseLab.Common
{
    public interface IIdValue<TId, TValue> :
        IId<TId>
    {
        TValue Value { get; }

        IIdValue<TId, TValue> Clone(TId id);
    }
}
