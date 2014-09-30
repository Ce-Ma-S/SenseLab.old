using SenseLab.Common.Nodes;
using SenseLab.Common.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SenseLab.Common.Environments
{
    public abstract class Device<T> :
        DeviceProvider<T>,
        IDevice
        where T : IDevice
    {
        public Device(Guid id, string name, string description = null,
            /*INode parent = null,*/ IList<T> children = null,
            IList<IRecordable> recordables = null)
            : base(id, name, description, /*parent,*/ children, recordables)
        {
        }

        public abstract bool IsAvailable { get; }

        protected virtual void OnIsAvailableChanged()
        {
            OnPropertyChanged(() => IsAvailable);
        }
    }


    public abstract class Device :
        Device<IDevice>,
        IDevice
    {
        public Device(Guid id, string name, string description = null,
            //INode parent = null,
            IList<IRecordable> recordables = null)
            : base(id, name, description, /*parent,*/ noChildren, recordables)
        {
        }

        private static readonly IDevice[] noChildren = new IDevice[0];
    }
}
