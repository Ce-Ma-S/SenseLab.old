using SenseLab.Common.Data;
using SenseLab.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SenseLab.Common.Environments
{
    public class EnvironmentsViewModel :
        ViewModel
    {
        public EnvironmentsViewModel()
        {
            Storages = new ObservableCollection<IItemStorage<IEnvironment, Guid>>();
        }

        public IList<IItemStorage<IEnvironment, Guid>> Storages { get; private set; }
    }
}
