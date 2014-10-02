namespace SenseLab.Common
{
    public class IdNameDescription<T> :
        IId<T>
    {
        public IdNameDescription(T id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public T Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
