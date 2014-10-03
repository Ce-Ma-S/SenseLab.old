using System.Runtime.Serialization;

namespace SenseLab.Common
{
    [DataContract]
    public class IdNameDescription<T> :
        IId<T>
    {
        public IdNameDescription(T id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        [DataMember]
        public T Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public string Description { get; private set; }
    }
}
