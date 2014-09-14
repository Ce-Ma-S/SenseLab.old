using SenseLab.Common.ViewModels.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.ViewModels.Nodes
{
    public interface ISpatialNodeViewModel : INodeViewModel
    {
        ILocationViewModel Location { get; }
    }
}
