namespace SenseLab.Common
{
    public static class IdHelper
    {
        public static IIdValue<TId, TValue> ToIdValue<TId, TValue>(this TValue value, TId id)
        {
            return new IdValue<TId, TValue>(id, value);
        }
    }
}
