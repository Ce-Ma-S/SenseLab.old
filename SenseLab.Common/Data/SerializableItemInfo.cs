using System;

namespace SenseLab.Common.Data
{
    /// <summary>
    /// Name (property name etc.) and identifier of an <see cref="ISerializableSubitems"/>.
    /// </summary>
    public class SerializableItemInfo
    {
        public SerializableItemInfo(string name, Type type, object id)
        {
            name.ValidateNonNullOrEmpty("name");
            type.ValidateNonNull("type");
            id.ValidateNonNull("id");
            Name = name;
            Type = type;
            Id = id;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
        public object Id { get; private set; }

        public override string ToString()
        {
            return string.Format("{1} {0} ({2})", Name, Type.Name, Id);
        }
    }
}
