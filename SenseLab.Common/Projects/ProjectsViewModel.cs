using SenseLab.Common.Environments;
using SenseLab.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SenseLab.Common.Projects
{
    public class ProjectsViewModel :
        ViewModel
    {
        public ProjectsViewModel()
        {
            var projectStorages = new ObservableCollection<IProjectStorage>();
            projectStorages.CollectionChanged += OnProjectStoragesChanged;
            ProjectStorages = projectStorages;
            OpenProjects = new ObservableCollection<IProject>();
        }

        public IList<IProjectStorage> ProjectStorages { get; private set; }
        public IProjectStorage DefaultProjectStorage
        {
            get { return defaultProjectStorage; }
            set
            {
                if (value != null && !ProjectStorages.Contains(value))
                    throw new ArgumentOutOfRangeException();
                SetProperty(() => DefaultProjectStorage, ref defaultProjectStorage, value);
            }
        }
        public IList<IProject> OpenProjects { get; private set; }
        public EnvironmentsViewModel Environments { get; private set; }

        private void OnProjectStoragesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DefaultProjectStorage != null && !ProjectStorages.Contains(DefaultProjectStorage))
                DefaultProjectStorage = null;
        }

        private IProjectStorage defaultProjectStorage;
    }
}
