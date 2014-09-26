using SenseLab.Common.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Environments
{
    public class EnvironmentsViewModel :
        ViewModel
    {
        public EnvironmentsViewModel()
        {
            Environments = new ObservableCollection<IEnvironments>();
        }

        public IList<IEnvironments> Environments { get; private set; }
    }
}
