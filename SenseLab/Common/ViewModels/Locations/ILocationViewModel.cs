using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseLab.Common.ViewModels.Locations
{
    public interface ILocationViewModel : IViewModel
    {
        string Text { get; }
    }
}
