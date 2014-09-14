using SenseLab.Common.Locations;
using System;

namespace SenseLab.Common.Events
{
    public class LocatableEventArgs : EventArgs, ILocatable
    {
        public LocatableEventArgs(ILocation location = null)
        {
            Location = location;
        }

        public ILocation Location { get; private set; }
    }
}
