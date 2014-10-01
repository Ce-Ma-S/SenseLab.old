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
            projects.CollectionChanged += OnProjectsChanged;
            Storages = projects;
            OpenProjects = new ObservableCollection<IProject>();
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
        public EnvironmentsViewModel Environments { get; private set; }

        private void OnProjectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DefaultStorage != null && !Storages.Contains(DefaultStorage))
                DefaultStorage = null;
        }

        private IProjectStorage defaultStorage;
    }
}
