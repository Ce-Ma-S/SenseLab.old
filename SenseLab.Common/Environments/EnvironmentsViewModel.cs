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
            EnvironmentStorages = new ObservableCollection<IEnvironmentStorage>();
        }

        public IList<IEnvironmentStorage> EnvironmentStorages { get; private set; }
    }
}
