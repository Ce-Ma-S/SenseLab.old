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
            Storages = new ObservableCollection<IEnvironmentStorage>();
        }

        public IList<IEnvironmentStorage> Storages { get; private set; }
    }
}
