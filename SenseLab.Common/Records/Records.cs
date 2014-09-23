using SenseLab.Common.Data;
using SenseLab.Common.Events;
using System.Collections.Generic;

namespace SenseLab.Common.Records
{
    public abstract class Records :
        NotifyPropertyChange,
        IRecords
    {
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(() => Name, ref name, value);
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
        
        public IItemStorage<IRecord> Storage { get; private set; }
        public ICollection<IRecordTransformer> WriteRecordTransformers { get; private set; }
        
        private string name;
        private string description;
    }
}
