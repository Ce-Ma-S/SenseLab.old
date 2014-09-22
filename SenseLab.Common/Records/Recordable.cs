using System;

namespace SenseLab.Common.Records
{
    public abstract class Recordable<T> :
        IRecordable
        where T : IRecorder
    {
        public Recordable(Guid id, IRecordType type, string name, string description = null)
        {
            type.ValidateNonNull("type");
            name.ValidateNonNullOrEmpty("name");
            Id = id;
            Type = type;
            Name = name;
            Description = description;
        }

        public Guid Id { get; private set; }
        public IRecordType Type { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public T CreateRecorder()
        {
            return DoCreateRecorder();
        }
        IRecorder IRecordable.CreateRecorder()
        {
            return CreateRecorder();
        }

        protected abstract T DoCreateRecorder();
    }
}
