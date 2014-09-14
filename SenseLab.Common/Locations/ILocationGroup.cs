using System;
using System.Collections.Generic;

namespace SenseLab.Common.Locations
{
    public interface ILocationGroup : ILocation
    {
        IEnumerable<ILocation> Locations { get; }
    }
}
