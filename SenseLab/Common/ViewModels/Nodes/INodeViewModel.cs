using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.ViewModels.Nodes
{
    public interface INodeViewModel : IViewModel
    {
        string Name { get; }
        string Description { get; }

        INodeViewModel Parent { get; }
        IEnumerable<INodeViewModel> Children { get; }
    }
}
