using SenseLab.Common.Events;
using System;

namespace SenseLab.Common.Records
{
    public class RecordGroup :
        NotifyPropertyChange,
        IRecordGroup
    {
        public RecordGroup(Guid id, string name, string description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(() => Name, ref name, value,
                    beforeChange: (n, v) => v.ValidateNonNullOrEmpty(n));
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                SetProperty(() => Description, ref description, value);
            }
        }

        private string name;
        private string description;
    }
}
