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
            recorder = new Lazy<T>(CreateRecorder);
        }

        public Guid Id { get; private set; }
        public IRecordType Type { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public T Recorder
        {
            get { return recorder.Value; }
        }
        IRecorder IRecordable.Recorder
        {
            get { return Recorder; }
        }

        protected abstract T CreateRecorder();

        private Lazy<T> recorder;
    }
}
