using Microsoft.Practices.ServiceLocation;
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
            var projects = new ObservableCollection<IProjectStorage>();
            projects.CollectionChanged += OnStoragesChanged;
            Storages = projects;
            OpenProjects = new ObservableCollection<IProject>();
            Environment = ServiceLocator.Current.GetInstance<IEnvironment>();
        }

        public IList<IProjectStorage> Storages { get; private set; }
        public IProjectStorage DefaultStorage
        {
            get { return defaultStorage; }
            set
            {
                if (value != null && !Storages.Contains(value))
                    throw new ArgumentOutOfRangeException();
                SetProperty(() => DefaultStorage, ref defaultStorage, value);
            }
        }
        public IList<IProject> OpenProjects { get; private set; }
        public IEnvironment Environment { get; private set; }

        private void OnStoragesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DefaultStorage != null && !Storages.Contains(DefaultStorage))
                DefaultStorage = null;
        }

        private IProjectStorage defaultStorage;
    }
}
